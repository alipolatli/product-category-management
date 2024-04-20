using MediatR;
using product_category.core.Domain.Mongo;

namespace product_category.api.Application.Commands.AttributeValues.UpdateAttributeValues;

public class UpdateAttributeValuesCommand : IRequest<InventoryUploadTrackingObject>
{
	public IEnumerable<UpdateAttributeValueRequest> AttributeValues { get; set; } = null!;
}

public class UpdateAttributeValueRequest
{
	public int CategoryId { get; set; }

	public int AttributeId { get; set; }

	public int AttributeValueId { get; set; }

	public string Name { get; set; }
}
