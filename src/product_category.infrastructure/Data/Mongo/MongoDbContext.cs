using MongoDB.Driver;
using product_category.core.Domain.Mongo;
using product_category.core.Domain.Mongo.SeedWork;

namespace product_category.infrastructure.Data.Mongo;

public class MongoDbContext
{
	private readonly IMongoDatabase _database;
	public MongoDbContext(string connectionString, string databaseName)
	{
		var client = new MongoClient(connectionString);
		_database = client.GetDatabase(databaseName);
	}

	public IMongoCollection<TDocument> GetCollection<TDocument>() where TDocument : MongoEntity
	{
		return _database.GetCollection<TDocument>(GetCollectionName<TDocument>());
	}

	private readonly Dictionary<Type, string> IndexNameMap = new Dictionary<Type, string>
		{
			{ typeof(InventoryUpload), "uploads" },
	};

	private string GetCollectionName<TDocument>() where TDocument : MongoEntity
	{
		var type = typeof(TDocument);

		if (IndexNameMap.TryGetValue(type, out var indexName))
			return indexName;

		throw new NotSupportedException($"Index name not supported for type {type.Name}");
	}
}
