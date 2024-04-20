using MongoDB.Bson;
using product_category.core.Domain.Mongo;

namespace product_category.core.Abstractions.Services;

public interface IInventoryUploadService
{
	Task<string> UploadAsync<T>(T uploadData);
	Task ReloadAsync<T>(string trackingType, ObjectId documentId, ObjectId trackingId, T reloadData);
	Task<InventoryUpload> TrackingAsync(string trackingId);
}
