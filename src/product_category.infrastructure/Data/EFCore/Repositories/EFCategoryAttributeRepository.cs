using Microsoft.EntityFrameworkCore;
using product_category.core.Abstractions.Repositories.EFCore;
using product_category.core.Domain.EFCore;

namespace product_category.infrastructure.Data.EFCore.Repositories;

public class EFCategoryAttributeRepository : EFGenericRepository<CategoryAttribute>, IEFCategoryAttributeRepository
{
	public EFCategoryAttributeRepository(EFCoreDbContext dbContext) : base(dbContext)
	{
	}

	public async Task<int> CountOfAttributes(int categoryId, bool? varianter = null, bool? grouped = null, CancellationToken cancellationToken = default)
				=> await _dbContext.CategoryAttributes
					.Where(ca => ca.CategoryId == categoryId &&
								 (!varianter.HasValue || ca.Varianter == varianter) &&
								 (!grouped.HasValue || ca.Grouped == grouped))
					.CountAsync(cancellationToken);
}
