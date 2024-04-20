using product_category.core.Domain.EFCore;

namespace product_category.core.Abstractions.Repositories.EFCore
{
	public interface IEFCategoryRepository : IEFGenericRepository<Category>
    {
		Task<bool> ContainsAttributesAsync(int categoryId, CancellationToken cancellationToken = default);
		Task<bool> ContainsSubCategoriesAsync(int categoryId, CancellationToken cancellationToken = default);

	}
}