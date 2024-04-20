using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using product_category.api.Application.Commands.Attributes.AddAttributes;
using product_category.api.Application.Commands.Attributes.DeleteAttributes;
using product_category.api.Application.Commands.Attributes.UpdateAttributes;
using product_category.api.Application.Commands.AttributeValues.AddAttributeValues;
using product_category.api.Application.Commands.AttributeValues.DeleteAttributeValues;
using product_category.api.Application.Commands.AttributeValues.UpdateAttributeValues;
using product_category.api.Application.Commands.Categories.AddCategories;
using product_category.api.Application.Commands.Categories.DeleteCategories;
using product_category.api.Application.Commands.Categories.UpdateCategories;
using product_category.core.Domain.Mongo;

namespace product_category.api.Apis;

public static class ProductCategoryApi
{
	public static IEndpointRouteBuilder MapProductCategoryApi(this IEndpointRouteBuilder app)
	{
		app.MapGet("trackings/{trackingId}", Trackings);

		app.MapPost("/categories", AddCategories);
		app.MapPut("/categories", UpdateCategories);
		app.MapDelete("/categories", DeleteCategories);

		app.MapPost("category-attributes", AddAttributes);
		app.MapPut("/category-attributes", UpdateAttributes);
		app.MapDelete("/category-attributes", DeleteAttributes);

		app.MapPost("category-attribute-values", AddAttributeValues);
		app.MapPut("/category-attribute-values", UpdateAttributeValues);
		app.MapDelete("/category-attribute-values", DeleteAttributeValues);

		return app;
	}

	static async Task<IResult> Trackings(string trackingId, [AsParameters] ProductCategoryServices services)
	{
		InventoryUpload inventoryUpload = await services.InventoryUploadService.TrackingAsync(trackingId);
		return TypedResults.Json(inventoryUpload.ToJson());
	}

	static async Task<IResult> AddCategories(AddCategoriesCommand request, [AsParameters] ProductCategoryServices services)
	{
		string trackingId = await services.InventoryUploadService.UploadAsync(request);
		return Results.Accepted(value: new { trackingId });
	}

	static async Task<IResult> UpdateCategories(UpdateCategoriesCommand request, [AsParameters] ProductCategoryServices services)
	{
		string trackingId = await services.InventoryUploadService.UploadAsync(request);
		return Results.Accepted(value: new { trackingId });
	}

	static async Task<IResult> DeleteCategories([FromBody] DeleteCategoriesCommand request, [AsParameters] ProductCategoryServices services)
	{
		string trackingId = await services.InventoryUploadService.UploadAsync(request);
		return Results.Accepted(value: new { trackingId });
	}




	static async Task<IResult> AddAttributes(AddAttributesCommand request, [AsParameters] ProductCategoryServices services)
	{
		string trackingId = await services.InventoryUploadService.UploadAsync(request);
		return Results.Accepted(value: new { trackingId });
	}

	static async Task<IResult> UpdateAttributes(UpdateAttributesCommand request, [AsParameters] ProductCategoryServices services)
	{
		string trackingId = await services.InventoryUploadService.UploadAsync(request);
		return Results.Accepted(value: new { trackingId });
	}

	static async Task<IResult> DeleteAttributes([FromBody] DeleteAttributesCommand request, [AsParameters] ProductCategoryServices services)
	{
		string trackingId = await services.InventoryUploadService.UploadAsync(request);
		return Results.Accepted(value: new { trackingId });
	}




	static async Task<IResult> AddAttributeValues(AddAttributeValuesCommand request, [AsParameters] ProductCategoryServices services)
	{
		string trackingId = await services.InventoryUploadService.UploadAsync(request);
		return Results.Accepted(value: new { trackingId });
	}

	static async Task<IResult> UpdateAttributeValues(UpdateAttributeValuesCommand request, [AsParameters] ProductCategoryServices services)
	{
		string trackingId = await services.InventoryUploadService.UploadAsync(request);
		return Results.Accepted(value: new { trackingId });
	}

	static async Task<IResult> DeleteAttributeValues([FromBody] DeleteAttributeValuesCommand request, [AsParameters] ProductCategoryServices services)
	{
		string trackingId = await services.InventoryUploadService.UploadAsync(request);
		return Results.Accepted(value: new { trackingId });
	}
}
