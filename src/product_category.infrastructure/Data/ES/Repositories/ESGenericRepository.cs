using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Elastic.Clients.Elasticsearch.QueryDsl;
using product_category.core.Abstractions.Repositories.ES;
using product_category.core.Domain.ES.SeedWork;
using System.Linq.Expressions;

namespace product_category.infrastructure.Data.ES.Repositories;

public class ESGenericRepository<T> : IESGenericRepository<T> where T : ESEntity
{
	protected readonly ElasticsearchClient _elasticsearchClient;
	protected string INDEX_NAME;

	public ESGenericRepository(ElasticsearchClient elasticsearchClient, CancellationToken cancellationToken = default)
	{
		_elasticsearchClient = elasticsearchClient;
		INDEX_NAME = ESIndexes.GetIndexName<T>();
	}


	public async Task<string?> AddAsync(T entity, CancellationToken cancellationToken = default)
	{
		var addResponse = await _elasticsearchClient.IndexAsync<T>(entity, cancellationToken);
		return addResponse.IsValidResponse ? addResponse.Id : null!;
	}

	public async Task<bool> BulkAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
	{
		BulkResponse response = await _elasticsearchClient.BulkAsync(s => s
										.Index(INDEX_NAME)
										.CreateMany(entities), cancellationToken
									);
		return response.IsValidResponse;
	}

	public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
	{
		DeleteResponse deleteResponse = await _elasticsearchClient.DeleteAsync(INDEX_NAME, id, cancellationToken);
		return deleteResponse.IsValidResponse;
	}

	public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
	{
		var getResponse = await _elasticsearchClient.GetAsync<T>(INDEX_NAME, new Id(id), cancellationToken);
		if (getResponse.IsValidResponse)
		{
			getResponse.Source!.Id = getResponse.Id;
			return getResponse.Source;
		}
		return null;
	}

	public async Task<T?> GetByIdsAsync(params (Expression<Func<T, string>> fieldSelector, string value)[] fieldSelectors)
	{
		var queries = fieldSelectors
												.Select(selector => new Action<QueryDescriptor<T>>(q =>
													q.Match(t => t
														.Field(selector.fieldSelector)
														.Query(selector.value)
														.Operator(Operator.And)
													)
												))
												.ToArray();

		var searchResponse = await _elasticsearchClient.SearchAsync<T>(s => s
			.Index(INDEX_NAME)
			.Query(q => q
				.Bool(b => b
					.Must(queries)
				)
			)
		);


		if (searchResponse.IsValidResponse && searchResponse.Documents.Any())
		{
			AssignIds(searchResponse.Hits);
			return searchResponse.Hits.Select(h => h.Source).FirstOrDefault();
		}
		return null;
	}

	public async Task<bool> IsExistsByIdAsync(Expression<Func<T, string>> fieldSelector, string value, CancellationToken cancellationToken = default)
	{
		var searchResponse = await _elasticsearchClient.SearchAsync<T>(s => s
												   .Index(INDEX_NAME)
												   .Query(q => q
													   .Match(t => t
														   .Field(fieldSelector)
														   .Query(value)
														   .Operator(Operator.And)
													   )
												   ), cancellationToken
											   );

		return searchResponse.IsValidResponse && searchResponse.Documents.Any();
	}

	public async Task<bool> IsExistsByIdsAsync(params (Expression<Func<T, string>> fieldSelector, string value)[] fieldSelectors)
	{
		var queries = fieldSelectors
						.Select(selector => new Action<QueryDescriptor<T>>(q =>
							q.Match(t => t
								.Field(selector.fieldSelector)
								.Query(selector.value)
								.Operator(Operator.And)
							)
						))
						.ToArray();

		var searchResponse = await _elasticsearchClient.SearchAsync<T>(s => s
			.Index(INDEX_NAME)
			.Size(1)
			.Query(q => q
				.Bool(b => b
					.Must(queries)
				)
			)
		);

		return searchResponse.IsValidResponse && searchResponse.Documents.Any();
	}

	public async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
	{
		var updateResponse = await _elasticsearchClient.UpdateAsync<T, T>(INDEX_NAME, entity.Id, u => u.Doc(entity), cancellationToken);
		return updateResponse.IsValidResponse;
	}

	protected void AssignIds(IReadOnlyCollection<Hit<T>> hits)
	{
		foreach (var item in hits)
		{
			item.Source!.Id = item.Id;
		}
	}
}
