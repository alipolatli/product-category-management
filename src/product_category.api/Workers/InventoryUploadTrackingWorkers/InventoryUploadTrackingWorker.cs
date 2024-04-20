using Confluent.Kafka;
using Microsoft.Extensions.Options;
using product_category.core.OptionsSettings;

namespace product_category.api.Workers.InventoryUploadTrackingWorkers;

public class InventoryUploadTrackingWorker : BackgroundService
{
	private readonly ILogger<InventoryUploadTrackingWorker> _logger;
	private readonly IConsumer<string, string> _consumer;
	private readonly IServiceProvider _serviceProvider;
	private readonly ApacheKafkaOptionsSettings _apacheKafkaOptionsSettings;

	public InventoryUploadTrackingWorker(IServiceProvider serviceProvider, ILogger<InventoryUploadTrackingWorker> logger, IOptions<ApacheKafkaOptionsSettings> options)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
		_apacheKafkaOptionsSettings = options.Value;

		var consumerConfig = new ConsumerConfig
		{
			BootstrapServers = _apacheKafkaOptionsSettings.BOOTSTRAPSERVERS,
			GroupId = _apacheKafkaOptionsSettings.GROUPID,
			AutoOffsetReset = AutoOffsetReset.Earliest
		};

		_consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
		_logger.LogInformation("[dbserver1.inventory.uploads] created by InventoryUploadWorker.");
	}

	public override Task StartAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("InventoryUploadWorker starting.");
		_consumer.Subscribe("dbserver1.inventory.uploads");
		_logger.LogInformation("InventoryUploadWorker subscribe to [dbserver1.inventory.uploads] topic.");
		return base.StartAsync(cancellationToken);
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("InventoryUploadWorker running.");
		await DoWork(stoppingToken);
	}

	private async Task DoWork(CancellationToken stoppingToken)
	{
		using (var scope = _serviceProvider.CreateScope())
		{
			IScopedInventoryUploadTrackingService scopedInventoryUploadTrackingService = scope.ServiceProvider.GetRequiredService<IScopedInventoryUploadTrackingService>();
			await scopedInventoryUploadTrackingService.TrackAndProcessAsync(_consumer, stoppingToken);
		}
	}

	public override async Task StopAsync(CancellationToken stoppingToken)
	{
		_consumer.Dispose();
		_logger.LogInformation("InventoryUploadWorker stopping.");
		await base.StopAsync(stoppingToken);
	}
}