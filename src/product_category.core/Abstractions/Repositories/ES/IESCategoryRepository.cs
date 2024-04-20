using product_category.core.Domain.ES;

namespace product_category.core.Abstractions.Repositories.ES;

public interface IESCategoryRepository : IESGenericRepository<Category>
{
	Task<IEnumerable<Category>> GetTopCategoriesAsync(CancellationToken cancellationToken = default);
	Task<IEnumerable<Category>> GetSubCategoriesAsync(int categoryId, CancellationToken cancellationToken = default);
	Task<Category?> GetParentCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
	Task<IEnumerable<Category>> SearchAsync(string categoryName, CancellationToken cancellationToken = default);
}
