using MediatR;
using product_category.core.Domain.Mongo;

namespace product_category.api.Application.Commands.AttributeValues.AddAttributeValues;

public class AddAttributeValuesCommand : IRequest<InventoryUploadTrackingObject>
{
	public IEnumerable<AddAttributeValueRequest> AttributeValues { get; set; } = null!;
}

public class AddAttributeValueRequest
{
	public int CategoryId { get; set; }

	public int AttributeId { get; set; }

	public string Name { get; set; } = null!;
}
