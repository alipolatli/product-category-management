using Elastic.Clients.Elasticsearch;
using product_category.core.Abstractions.Repositories.ES;
using product_category.core.Domain.ES;
using System.Text.Json.Serialization;

namespace product_category.infrastructure.Data.ES.Repositories;

public class ESCategoryAttributeValueRepository : ESGenericRepository<CategoryAttributeValue>, IESCategoryAttributeValueRepository
{
	[JsonConstructor]
	public ESCategoryAttributeValueRepository(ElasticsearchClient elasticsearchClient) : base(elasticsearchClient)
	{
	}

	public async Task<IEnumerable<CategoryAttributeValue>> GetAttributeValuesAsync(int attributeId, CancellationToken cancellationToken = default)
	{
		SearchResponse<CategoryAttributeValue> response = await _elasticsearchClient.SearchAsync<CategoryAttributeValue>(s => s
						  .Size(10000)
						  .Index(INDEX_NAME)
						  .Query(q => q
							  .Bool(b => b
								  .Must(m => m
									  .Term(t => t
										  .Field(f => f.CategoryAttributeId)
										  .Value(attributeId)
										   )
										)
									 )
								), cancellationToken
							  );
		return response.IsValidResponse ? response.Documents : Enumerable.Empty<CategoryAttributeValue>();
	}
}