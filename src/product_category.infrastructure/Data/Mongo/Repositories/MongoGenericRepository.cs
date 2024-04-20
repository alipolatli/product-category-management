using MongoDB.Bson;
using MongoDB.Driver;
using product_category.core.Abstractions.Repositories.Mongo;
using product_category.core.Domain.Mongo.SeedWork;
using System.Linq.Expressions;

namespace product_category.infrastructure.Data.Mongo.Repositories;

public class MongoGenericRepository<TDocument> : IMongoGenericRepository<TDocument> where TDocument : MongoEntity
{
	private readonly IMongoCollection<TDocument> _mongoCollection;

	public MongoGenericRepository(MongoDbContext dbContext)
	{
		_mongoCollection = dbContext.GetCollection<TDocument>();
	}

	public virtual IQueryable<TDocument> AsQueryable()
	{
		return _mongoCollection.AsQueryable();
	}

	public virtual IEnumerable<TDocument> FilterBy(
		Expression<Func<TDocument, bool>> filterExpression)
	{
		return _mongoCollection.Find(filterExpression).ToEnumerable();
	}

	public virtual IEnumerable<TProjected> FilterBy<TProjected>(
		Expression<Func<TDocument, bool>> filterExpression,
		Expression<Func<TDocument, TProjected>> projectionExpression)
	{
		return _mongoCollection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
	}

	public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
	{
		return _mongoCollection.Find(filterExpression).FirstOrDefault();
	}

	public virtual Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
	{
		return Task.Run(() => _mongoCollection.Find(filterExpression).FirstOrDefaultAsync());
	}

	public virtual TDocument FindById(string id)
	{
		var objectId = new ObjectId(id);
		var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
		return _mongoCollection.Find(filter).SingleOrDefault();
	}

	public virtual Task<TDocument> FindByIdAsync(string id)
	{
		return Task.Run(() =>
		{
			var objectId = new ObjectId(id);
			var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
			return _mongoCollection.Find(filter).SingleOrDefaultAsync();
		});
	}

	public virtual void InsertOne(TDocument document)
	{
		_mongoCollection.InsertOne(document);
	}

	public virtual async Task InsertOneAsync(TDocument document)
	{
		await _mongoCollection.InsertOneAsync(document);
	}

	public void InsertMany(ICollection<TDocument> documents)
	{
		_mongoCollection.InsertMany(documents);
	}

	public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
	{
		await _mongoCollection.InsertManyAsync(documents);
	}

	public void ReplaceOne(TDocument document)
	{
		document.ModifiedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();
		var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
		_mongoCollection.FindOneAndReplace(filter, document);
	}

	public virtual async Task ReplaceOneAsync(TDocument document)
	{
		document.ModifiedAt = DateTimeOffset.Now.ToUnixTimeMilliseconds();
		await _mongoCollection.ReplaceOneAsync(i => i.Id == document.Id, document);
	}

	public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
	{
		_mongoCollection.FindOneAndDelete(filterExpression);
	}

	public Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
	{
		return Task.Run(() => _mongoCollection.FindOneAndDeleteAsync(filterExpression));
	}

	public void DeleteById(string id)
	{
		var objectId = new ObjectId(id);
		var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
		_mongoCollection.FindOneAndDelete(filter);
	}

	public Task DeleteByIdAsync(string id)
	{
		return Task.Run(() =>
		{
			var objectId = new ObjectId(id);
			var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
			_mongoCollection.FindOneAndDeleteAsync(filter);
		});
	}

	public void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
	{
		_mongoCollection.DeleteMany(filterExpression);
	}

	public Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
	{
		return Task.Run(() => _mongoCollection.DeleteManyAsync(filterExpression));
	}
}
