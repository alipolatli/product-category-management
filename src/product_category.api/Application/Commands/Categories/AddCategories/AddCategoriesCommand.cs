using MediatR;
using product_category.core.Domain.Mongo;

namespace product_category.api.Application.Commands.Categories.AddCategories;

public class AddCategoriesCommand : IRequest<InventoryUploadTrackingObject>
{
	public IEnumerable<AddCategoryRequest> Categories { get; set; } = null!;
}

public class AddCategoryRequest
{
	public string Name { get; set; }
	public int? ParentId { get; set; }
	public ICollection<AddCategoryRequest>? SubCategories { get; set; }
}