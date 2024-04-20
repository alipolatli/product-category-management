using MediatR;
using product_category.core.Domain.Mongo;

namespace product_category.api.Application.Commands.AttributeValues.DeleteAttributeValues;

public class DeleteAttributeValuesCommand : IRequest<InventoryUploadTrackingObject>
{
	public IEnumerable<DeleteAttributeValueRequest> AttributeValues { get; set; } = null!;
}

public class DeleteAttributeValueRequest
{
	public int CategoryId { get; set; }

	public int AttributeId { get; set; }

	public int AttributeValueId { get; set; }
}
