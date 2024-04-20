using product_category.core.Domain.ES;

namespace product_category.core.Abstractions.Repositories.ES;

public interface IESCategoryAttributeValueRepository : IESGenericRepository<CategoryAttributeValue>
{
	Task<IEnumerable<CategoryAttributeValue>> GetAttributeValuesAsync(int attributeId, CancellationToken cancellationToken = default);
}
