using MediatR;
using product_category.core.Domain.Mongo;

namespace product_category.api.Application.Commands.Attributes.DeleteAttributes;

public class DeleteAttributesCommand : IRequest<InventoryUploadTrackingObject>
{
	public ICollection<DeleteAttributeRequest> Attributes { get; set; } = null!;
}

public class DeleteAttributeRequest
{
	public int CategoryId { get; set; }
	public int AttributeId { get; set; }
}
