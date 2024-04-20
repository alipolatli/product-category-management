using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace product_category.core.Domain.Mongo.SeedWork;

public abstract class MongoEntity
{
	[BsonId]
	public ObjectId Id { get; set; }

	[BsonIgnoreIfNull]
	public long ModifiedAt { get; set; }
}
