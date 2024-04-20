using Microsoft.EntityFrameworkCore;
using product_category.core.Abstractions.Repositories.EFCore;
using product_category.core.Domain.EFCore;

namespace product_category.infrastructure.Data.EFCore.Repositories;

public class EFCategoryRepository : EFGenericRepository<Category>, IEFCategoryRepository
{
	public EFCategoryRepository(EFCoreDbContext dbContext) : base(dbContext)
	{
	}

	public async Task<bool> ContainsAttributesAsync(int categoryId, CancellationToken cancellationToken = default)
		=> await _dbContext.CategoryAttributes.AnyAsync(c => c.CategoryId == categoryId, cancellationToken = default);

	public async Task<bool> ContainsSubCategoriesAsync(int categoryId, CancellationToken cancellationToken = default)
		=> await _dbContext.Categories.AnyAsync(c => c.ParentId == categoryId, cancellationToken);
}