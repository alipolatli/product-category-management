using Confluent.Kafka;
using product_category.api.Application.Commands.Attributes.AddAttributes;
using product_category.api.Application.Commands.Attributes.DeleteAttributes;
using product_category.api.Application.Commands.Attributes.UpdateAttributes;
using product_category.api.Application.Commands.AttributeValues.AddAttributeValues;
using product_category.api.Application.Commands.AttributeValues.DeleteAttributeValues;
using product_category.api.Application.Commands.AttributeValues.UpdateAttributeValues;
using product_category.api.Application.Commands.Categories.AddCategories;
using product_category.api.Application.Commands.Categories.DeleteCategories;
using product_category.api.Application.Commands.Categories.UpdateCategories;
using product_category.core.Abstractions.Services;
using MongoDB.Bson.Serialization;
using Polly;
using System.Text.Json;
using System.Text.Json.Serialization;
using MediatR;
using product_category.core.Domain.Mongo;

namespace product_category.api.Workers.InventoryUploadTrackingWorkers;

public class ScopedInventoryUploadTrackingService : IScopedInventoryUploadTrackingService
{
	private readonly ILogger<ScopedInventoryUploadTrackingService> _logger;
	private readonly IMediator _mediator;
	private readonly IInventoryUploadService _inventoryUploadService;

	public ScopedInventoryUploadTrackingService(ILogger<ScopedInventoryUploadTrackingService> logger, IMediator mediator, IInventoryUploadService inventoryUploadService)
	{
		_logger = logger;
		_mediator = mediator;
		_inventoryUploadService = inventoryUploadService;
	}

	public async Task TrackAndProcessAsync(IConsumer<string, string> consumer, CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			var result = consumer.Consume(TimeSpan.FromMilliseconds(500));
			if (result != null)
			{
				await HandleMessage(result.Message.Value, stoppingToken);
				consumer.Commit(result);
			}

			await Task.Delay(100, stoppingToken);
			_logger.LogInformation("Scoped Processing Service is working.");
		}
	}

	protected virtual async Task HandleMessage(string message, CancellationToken stoppingToken)
	{
		var debeziumMessage = JsonSerializer.Deserialize<DebeziumMessage>(message);
		InventoryUpload? inventoryUpload = debeziumMessage!.Op.Equals("c")
			? BsonSerializer.Deserialize<InventoryUpload>(debeziumMessage.After)
			: null;
		if (inventoryUpload == null) return;

		InventoryUploadTrackingObject? inventoryUploadTrackingObject = null;
		await CreateRetryPolicy().ExecuteAsync(async () =>
			{
				if (Enum.TryParse<CommandTypes>(inventoryUpload.Type, out var commandType))
				{
					object? commandData = BsonSerializer.Deserialize(inventoryUpload.Data, GetCommandType(commandType));
					try
					{
						inventoryUploadTrackingObject = await _mediator.Send(commandData) as InventoryUploadTrackingObject;
					}
					catch (Exception ex)
					{
						throw;
					}

					if (inventoryUploadTrackingObject == null)
					{
						_logger.LogWarning("inventoryUploadTrackingObject is null. Retrying...");
						throw new InvalidOperationException("inventoryUploadTrackingObject is null");
					}
					await _inventoryUploadService.ReloadAsync(inventoryUpload.Type, inventoryUpload.Id, inventoryUpload.TrackingId, inventoryUploadTrackingObject);
				}
				else
				{
					_logger.LogError($"Unsupported command type: {inventoryUpload.Type}");
					throw new ArgumentOutOfRangeException(nameof(inventoryUpload.Type), inventoryUpload.Type, "Unsupported command type");
				}
			});

		if (inventoryUploadTrackingObject == null)
		{
			inventoryUploadTrackingObject = new InventoryUploadTrackingObject();
			inventoryUploadTrackingObject.AddItem(inventoryUpload.Data, new Dictionary<string, string[]> { { "System", new string[] { "System error. Try again." } } });
			_logger.LogWarning("inventoryUploadTrackingObject is still null after retries. System error.");
			await _inventoryUploadService.ReloadAsync(inventoryUpload.Type, inventoryUpload.Id, inventoryUpload.TrackingId, inventoryUploadTrackingObject);
		}
	}

	private Type GetCommandType(CommandTypes commandType)
	{
		return commandType switch
		{
			CommandTypes.AddCategoriesCommand => typeof(AddCategoriesCommand),
			CommandTypes.UpdateCategoriesCommand => typeof(UpdateCategoriesCommand),
			CommandTypes.DeleteCategoriesCommand => typeof(DeleteCategoriesCommand),
			CommandTypes.AddAttributesCommand => typeof(AddAttributesCommand),
			CommandTypes.UpdateAttributesCommand => typeof(UpdateAttributesCommand),
			CommandTypes.DeleteAttributesCommand => typeof(DeleteAttributesCommand),
			CommandTypes.AddAttributeValuesCommand => typeof(AddAttributeValuesCommand),
			CommandTypes.UpdateAttributeValuesCommand => typeof(UpdateAttributeValuesCommand),
			CommandTypes.DeleteAttributeValuesCommand => typeof(DeleteAttributeValuesCommand),
			_ => throw new ArgumentOutOfRangeException(nameof(commandType), commandType, "Unsupported command type"),
		};
	}

	private AsyncPolicy CreateRetryPolicy()
	{
		return Policy
			.Handle<Exception>()
			.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2), (exception, timeSpan, retryCount, context) =>
			{
				_logger.LogError($"Error processing message. Retry count: {retryCount}. Waiting for {timeSpan.TotalSeconds} seconds. Exception: {exception}");
			});
	}
}

public enum CommandTypes
{
	AddCategoriesCommand,
	UpdateCategoriesCommand,
	DeleteCategoriesCommand,
	AddAttributesCommand,
	UpdateAttributesCommand,
	DeleteAttributesCommand,
	AddAttributeValuesCommand,
	UpdateAttributeValuesCommand,
	DeleteAttributeValuesCommand,
	AddBrandsCommand,
	UpdateBrandsCommand,
	DeleteBrandsCommand
}

public class DebeziumMessage
{
	[JsonPropertyName("after")]
	public string After { get; set; } = null!;

	[JsonPropertyName("op")]
	public string Op { get; set; } = null!;
}
