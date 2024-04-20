using product_category.core.Abstractions.Repositories.Mongo;
using product_category.core.Domain.Mongo;

namespace product_category.infrastructure.Data.Mongo.Repositories;

public class MongoInventoryUploadRepository : MongoGenericRepository<InventoryUpload>, IMongoInventoryUploadRepository
{
	public MongoInventoryUploadRepository(MongoDbContext dbContext) : base(dbContext)
	{
	}
}
