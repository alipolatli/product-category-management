using MediatR;
using product_category.core.Domain.Mongo;

namespace product_category.api.Application.Commands.Categories.DeleteCategories;

public class DeleteCategoriesCommand : IRequest<InventoryUploadTrackingObject>
{
	public ICollection<DeleteCategoryRequest> Categories { get; set; }
}

public class DeleteCategoryRequest
{
	public int CategoryId { get; set; }
}