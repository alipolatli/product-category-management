using product_category.core.Domain.EFCore;

namespace product_category.core.Abstractions.Repositories.EFCore
{
	public interface IEFCategoryAttributeValueRepository : IEFGenericRepository<CategoryAttributeValue>
    {
        Task<bool> ExistCategoryAttributeValueAsync(int categoryId, int attributeId, int attributeValueId,CancellationToken cancellationToken = default);
		Task<bool> ExistCategoryAttributeValueAsync(int categoryId, int attributeId, int attributeValueId,string attributeValueName, CancellationToken cancellationToken = default);
	}
}
