using FluentValidation;
using product_category.api.Application.Services;
using product_category.core;
using product_category.core.Abstractions.Services;
using System.Reflection;
using product_category.infrastructure;
using product_category.api.Workers.InventoryUploadTrackingWorkers;

namespace product_category.api.Application;

public static class ApplicationExtension
{
	public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
	{
		#region Infrastructure
		builder.Services.AddInfrastructure(builder.Configuration);
		#endregion

		#region MediatR
		builder.Services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
		});
		#endregion

		#region FluentValidation
		ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
		builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
		#endregion

		#region TenantProvider
		builder.Services.AddScoped<TenantProvider>();
		#endregion

		#region Services
		builder.Services.AddScoped<IInventoryUploadService, InventoryUploadService>();
		#endregion

		#region BackgroundServices (Workers)
		builder.Services.AddHostedService<InventoryUploadTrackingWorker>();
		builder.Services.AddScoped<IScopedInventoryUploadTrackingService, ScopedInventoryUploadTrackingService>();
		#endregion

		#region HttpContext
		builder.Services.AddHttpContextAccessor();
		#endregion
		return builder;
	}
}