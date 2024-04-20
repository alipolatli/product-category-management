using MediatR;
using product_category.core.Domain.Mongo;

namespace product_category.api.Application.Commands.Attributes.UpdateAttributes;

public class UpdateAttributesCommand : IRequest<InventoryUploadTrackingObject>
{
	public ICollection<UpdateAttributeRequest> Attributes { get; set; } = null!;
}

public class UpdateAttributeRequest
{
	public int CategoryId { get; set; }

	public int AttributeId { get; set; }

	public string Name { get; set; } = null!;
}