using MediatR;
using product_category.core.Domain.Mongo;

namespace product_category.api.Application.Commands.Categories.UpdateCategories;

public class UpdateCategoriesCommand : IRequest<InventoryUploadTrackingObject>
{
	public IEnumerable<UpdateCategoryRequest> Categories { get; set; } = null!;
}

public class UpdateCategoryRequest
{
	public int CategoryId { get; set; }
	public int? MoveParentCategoryId { get; set; }
	public string? Name { get; set; }
}