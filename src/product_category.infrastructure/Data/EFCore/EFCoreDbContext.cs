using Microsoft.EntityFrameworkCore;
using product_category.core;
using product_category.core.Domain.EFCore;
using product_category.core.Domain.EFCore.SeedWork;
using System.Reflection;

namespace product_category.infrastructure.Data.EFCore;

public class EFCoreDbContext : DbContext, IUnitOfWork
{
	private readonly TenantProvider _tenantProvider;
	private readonly Guid TENANT_ID;

	public EFCoreDbContext(TenantProvider tenantProvider, DbContextOptions<EFCoreDbContext> dbContextOptions) : base(dbContextOptions)
	{
		_tenantProvider = tenantProvider ?? throw new ArgumentNullException(nameof(TenantProvider));
		TENANT_ID = tenantProvider.GetTenantId();
	}

	public DbSet<Category> Categories { get; set; }
	public DbSet<CategoryAttribute> CategoryAttributes { get; set; }
	public DbSet<CategoryAttributeValue> CategoryAttributeValues { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
			.AddTenancyProperties()
			.AddTenancyQueries(TENANT_ID);
	}

	public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
	{
		await base.SaveChangesAsync(cancellationToken);
		return true;
	}

}
