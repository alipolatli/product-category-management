using product_category.core.Domain.ES;

namespace product_category.core.Abstractions.Repositories.ES;

public interface IESCategoryAttributeRepository : IESGenericRepository<CategoryAttribute>
{
	Task<IEnumerable<CategoryAttribute>> GetAttributesAsync(int categoryId, CancellationToken cancellationToken = default);
}
