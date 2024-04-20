using Microsoft.EntityFrameworkCore;
using product_category.core.Domain.EFCore.SeedWork;
using System.Linq.Expressions;

namespace product_category.infrastructure.Data.EFCore;

public static class ModelBuilderExtension
{
	public static ModelBuilder AddTenancyProperties(this ModelBuilder builder)
	{
		foreach (var entityType in builder.Model.GetEntityTypes())
		{
			var entityTypeBuilder = builder.Entity(entityType.ClrType);

			if (typeof(ITenancy).IsAssignableFrom(entityType.ClrType))
			{
				entityTypeBuilder.Property<Guid>("tenantId")
								.IsRequired();
			}
		}
		return builder;
	}

	public static ModelBuilder AddTenancyQueries(this ModelBuilder builder, Guid tenantId)
	{
		foreach (var entityType in builder.Model.GetEntityTypes())
		{
			if (typeof(ITenancy).IsAssignableFrom(entityType.ClrType))
			{
				var entityTypeBuilder = builder.Entity(entityType.ClrType);

				// Create parameter expression
				var parameter = Expression.Parameter(entityType.ClrType, "entity");

				// Create tenantId expression
				var tenantIdExpression = Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(Guid) }, parameter, Expression.Constant("tenantId"));
				var tenantBody = Expression.Equal(tenantIdExpression, Expression.Constant(tenantId));

				// Add query filter
				entityTypeBuilder.HasQueryFilter(Expression.Lambda(tenantBody, parameter));
			}
		}
		return builder;
	}
}
