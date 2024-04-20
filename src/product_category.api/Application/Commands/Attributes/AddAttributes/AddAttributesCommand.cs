using MediatR;
using product_category.core.Domain.Mongo;

namespace product_category.api.Application.Commands.Attributes.AddAttributes;

public class AddAttributesCommand : IRequest<InventoryUploadTrackingObject>
{
	public ICollection<AddAttributeRequest> Attributes { get; set; } = null!;
}

public class AddAttributeRequest
{
	public int CategoryId { get; set; }

	public string Name { get; set; } = null!;

	public bool Varianter { get; set; }

	public bool? Grouped { get; set; }
}