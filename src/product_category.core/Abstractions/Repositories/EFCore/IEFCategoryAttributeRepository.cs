using product_category.core.Domain.EFCore;

namespace product_category.core.Abstractions.Repositories.EFCore
{
	public interface IEFCategoryAttributeRepository : IEFGenericRepository<CategoryAttribute>
    {
		Task<int> CountOfAttributes(int categoryId, bool? varianter = null, bool? grouped = null, CancellationToken cancellationToken = default);
	}
}
