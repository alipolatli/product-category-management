using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using product_category.core.Abstractions.Repositories.EFCore;
using product_category.core.Abstractions.Repositories.ES;
using product_category.core.Abstractions.Repositories.Mongo;
using product_category.core.OptionsSettings;
using product_category.infrastructure.Data.EFCore;
using product_category.infrastructure.Data.EFCore.Repositories;
using product_category.infrastructure.Data.ES.Repositories;
using product_category.infrastructure.Data.Mongo;
using product_category.infrastructure.Data.Mongo.Repositories;

namespace product_category.infrastructure;

public static class InfrastructureExtension
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		#region EF
		services.AddDbContext<EFCoreDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("POSTGRES")!));
		#endregion

		#region ES
		services.AddSingleton(sp =>
		{
			return new ElasticsearchClient(new Uri(configuration.GetConnectionString("ELASTICSEARCH")!));
		});
		#endregion

		#region Mongo
		services.AddSingleton<MongoDbContext>(_ =>
		{
			var connectionString = configuration.GetConnectionString("MONGODB") ?? throw new ArgumentNullException("MONGODB");
			var databaseName = configuration["CONFIGURATIONS:MONGODB:DATABASENAME"] ?? throw new ArgumentNullException("CONFIGURATIONS:MONGODB:DATABASENAME");
			return new MongoDbContext(connectionString, databaseName);
		});
		#endregion

		#region OptionsSettings
		services.Configure<ApacheKafkaOptionsSettings>(configuration.GetSection("CONFIGURATIONS:APACHEKAFKA"));
		#endregion

		#region Repositories
		services.AddScoped<IMongoInventoryUploadRepository, MongoInventoryUploadRepository>();
		services.AddScoped<IEFCategoryRepository, EFCategoryRepository>();
		services.AddScoped<IEFCategoryAttributeRepository, EFCategoryAttributeRepository>();
		services.AddScoped<IEFCategoryAttributeValueRepository, EFCategoryAttributeValueRepository>();
		services.AddScoped<IESCategoryRepository, ESCategoryRepository>();
		services.AddScoped<IESCategoryAttributeRepository, ESCategoryAttributeRepository>();
		services.AddScoped<IESCategoryAttributeValueRepository, ESCategoryAttributeValueRepository>();
		#endregion

		return services;
	}
}