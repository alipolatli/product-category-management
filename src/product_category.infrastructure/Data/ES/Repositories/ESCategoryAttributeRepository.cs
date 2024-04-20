using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Bulk;
using Elastic.Clients.Elasticsearch.QueryDsl;
using product_category.core.Abstractions.Repositories.ES;
using product_category.core.Domain.ES;
using System.Text.Json.Serialization;

namespace product_category.infrastructure.Data.ES.Repositories;

public class ESCategoryAttributeRepository : ESGenericRepository<CategoryAttribute>, IESCategoryAttributeRepository
{
	[JsonConstructor]
	public ESCategoryAttributeRepository(ElasticsearchClient elasticsearchClient) : base(elasticsearchClient)
	{

	}

	public async Task<IEnumerable<CategoryAttribute>> GetAttributesAsync(int categoryId, CancellationToken cancellationToken = default)
	{
		SearchResponse<CategoryAttribute> response = await _elasticsearchClient.SearchAsync<CategoryAttribute>(s => s
						.Index(INDEX_NAME)
						.Size(1000)
						.Query(q => q
							.Bool(b => b
								.Must(m => m
									.Term(t => t
										.Field(f => f.CategoryId)
										.Value(categoryId)
									)
								)
							)
						),cancellationToken);

		return response.IsValidResponse ? response.Documents : Enumerable.Empty<CategoryAttribute>();
	}
}