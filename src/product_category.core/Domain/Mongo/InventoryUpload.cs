using MongoDB.Bson;
using product_category.core.Domain.Mongo.SeedWork;

namespace product_category.core.Domain.Mongo;

public class InventoryUpload : MongoEntity
{
	public string Type { get; set; }
	public ObjectId TrackingId { get; set; }
	public BsonDocument Data { get; set; }

	public InventoryUpload(string type, ObjectId trackingId, BsonDocument data, ObjectId? id = null)
	{
		if (id.HasValue)
			Id = id.Value;
		Type = type;
		TrackingId = trackingId;
		Data = data;
	}
}
