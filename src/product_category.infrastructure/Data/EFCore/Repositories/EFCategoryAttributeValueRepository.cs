using Microsoft.EntityFrameworkCore;
using product_category.core.Abstractions.Repositories.EFCore;
using product_category.core.Domain.EFCore;

namespace product_category.infrastructure.Data.EFCore.Repositories
{
	public class EFCategoryAttributeValueRepository : EFGenericRepository<CategoryAttributeValue>, IEFCategoryAttributeValueRepository
	{
		public EFCategoryAttributeValueRepository(EFCoreDbContext dbContext) : base(dbContext)
		{
		}

		public async Task<bool> ExistCategoryAttributeValueAsync(int categoryId, int attributeId, int attributeValueId, CancellationToken cancellationToken = default)
			=> await _dbContext.Categories
				  .Join(_dbContext.CategoryAttributes, c => c.Id, ca => ca.CategoryId, (c, ca) => new { c, ca })
				  .Join(_dbContext.CategoryAttributeValues, firstJoin => firstJoin.ca.Id, cav => cav.CategoryAttributeId, (firstJoin, cav) => new { firstJoin, cav })
				  .AnyAsync(entry =>
					  entry.firstJoin.c.Id == categoryId &&
					  entry.firstJoin.ca.Id == attributeId &&
					  entry.cav.Id == attributeValueId,
					  cancellationToken);

		public async Task<bool> ExistCategoryAttributeValueAsync(int categoryId, int attributeId, int attributeValueId, string attributeValueName, CancellationToken cancellationToken = default)
				=> await _dbContext.Categories
					.Join(_dbContext.CategoryAttributes, c => c.Id, ca => ca.CategoryId, (c, ca) => new { c, ca })
					.Join(_dbContext.CategoryAttributeValues, firstJoin => firstJoin.ca.Id, cav => cav.CategoryAttributeId, (firstJoin, cav) => new { firstJoin, cav })
					.AnyAsync(entry =>
						entry.firstJoin.c.Id == categoryId &&
						entry.firstJoin.ca.Id == attributeId &&
						entry.cav.Id == attributeValueId &&
						entry.cav.Name == attributeValueName,
						cancellationToken);
	}
}
