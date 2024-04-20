using MongoDB.Bson;
using product_category.core.Abstractions.Repositories.Mongo;
using product_category.core.Abstractions.Services;
using product_category.core.Domain.Mongo;
using product_category.core.Extensions;

namespace product_category.api.Application.Services;

public class InventoryUploadService : IInventoryUploadService
{
	private readonly IMongoInventoryUploadRepository _inventoryUploadRepository;
	public InventoryUploadService(IMongoInventoryUploadRepository inventoryUploadRepository)
	{
		_inventoryUploadRepository = inventoryUploadRepository;
	}

	public async Task ReloadAsync<T>(string trackingType, ObjectId documentId, ObjectId trackingId, T reloadData)
	{
		await _inventoryUploadRepository.ReplaceOneAsync(new InventoryUpload(trackingType, trackingId, reloadData.ToBsonDocument(), documentId));
	}

	public async Task<InventoryUpload> TrackingAsync(string trackingId)
	{
		var inventoryUplaod = await _inventoryUploadRepository.FindOneAsync(i => i.TrackingId == ObjectId.Parse(trackingId));
		return inventoryUplaod;
	}

	public async Task<string> UploadAsync<T>(T uploadData)
	{
		ObjectId trackingId = ObjectId.GenerateNewId();
		await _inventoryUploadRepository.InsertOneAsync(new InventoryUpload(uploadData!.GetGenericTypeName(), trackingId, uploadData.ToBsonDocument()));
		return trackingId.ToString();
	}
}
