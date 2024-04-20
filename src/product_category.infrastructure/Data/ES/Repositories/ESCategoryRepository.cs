using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using product_category.core.Abstractions.Repositories.ES;
using product_category.core.Domain.ES;

namespace product_category.infrastructure.Data.ES.Repositories;

public class ESCategoryRepository : ESGenericRepository<Category>, IESCategoryRepository
{
	public ESCategoryRepository(ElasticsearchClient elasticsearchClient) : base(elasticsearchClient)
	{

	}

	public async Task<IEnumerable<Category>> GetTopCategoriesAsync(CancellationToken cancellationToken = default)
	{
		SearchResponse<Category> response = await _elasticsearchClient.SearchAsync<Category>(s => s
						.Index(INDEX_NAME)
						.Size(10000)
						.Query(q => q
							.Bool(b => b
								.MustNot(mn => mn
									.Exists(e => e.Field(f => f.ParentId))
								)
							)
						), cancellationToken
					);
		return response.IsValidResponse ? response.Documents : Enumerable.Empty<Category>();
	}

	public async Task<Category?> GetParentCategoryAsync(int categoryId, CancellationToken cancellationToken = default)
	{
		SearchResponse<Category>? thisCategoryResponse = await _elasticsearchClient.SearchAsync<Category>(s => s.Index(INDEX_NAME)
						.Query(q => q
							.Term(t => t
								.Field(f => f.CategoryId)
								.Value(categoryId)
							)
						), cancellationToken
					);

		if (thisCategoryResponse.Total < 1)
			return null;

		int? parentId = thisCategoryResponse.Documents.FirstOrDefault()?.ParentId;
		if (!parentId.HasValue)
			return null;

		SearchResponse<Category> parentCategoryResponse = await _elasticsearchClient.SearchAsync<Category>(s => s.Index(INDEX_NAME)
				.Query(q => q
					.Term(t => t
						.Field(f => f.CategoryId)
						.Value(parentId.Value)
						)
					), cancellationToken
			);
		return parentCategoryResponse.Total >= 1 ? parentCategoryResponse.Documents.FirstOrDefault() : null;
	}

	public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int categoryId, CancellationToken cancellationToken = default)
	{
		SearchResponse<Category> response = await _elasticsearchClient.SearchAsync<Category>(s => s.Index(INDEX_NAME)
						.Size(10000) //becasuse I want all of them
						.Query(q => q
							.Term(t => t
								.Field(f => f.ParentId)
								.Value(categoryId)
							)
						), cancellationToken
					);

		return response.IsValidResponse ? response.Documents : Enumerable.Empty<Category>();
	}

	public async Task<IEnumerable<Category>> SearchAsync(string categoryName, CancellationToken cancellationToken = default)
	{
		SearchResponse<Category> response = await _elasticsearchClient.SearchAsync<Category>(s => s
					.Index(INDEX_NAME)
					.Size(100)
					.Query(q => q
						.Bool(b => b
							.Should(sh => sh
								.Match(m => m
									.Field(f => f.Name)
									.Query(categoryName)
									.Operator(Operator.Or)
									.Fuzziness(new Fuzziness("auto"))
									.PrefixLength(1)
									.MaxExpansions(10)
								),
								sh => sh
								.MatchPhrasePrefix(mp => mp
									.Field(f => f.Name)
									.Query(categoryName)
									.MaxExpansions(10)
								),
								sh => sh
								.Wildcard(w => w
									.Field(f => f.Name.Suffix("keyword"))
									.Value($"*{categoryName}*")
								)
							)
						)
					), cancellationToken
				);

		return response.IsValidResponse ? response.Documents : Enumerable.Empty<Category>();
	}
}