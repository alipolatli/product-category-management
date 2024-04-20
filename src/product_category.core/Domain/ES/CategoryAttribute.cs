using product_category.core.Domain.ES.SeedWork;
using System.Text.Json.Serialization;

namespace product_category.core.Domain.ES;

public class CategoryAttribute : ESEntity
{
	[JsonPropertyName("categoryId")]
	public int CategoryId { get; set; }

	[JsonPropertyName("attributeId")]
	public int AttributeId { get; set; }

	[JsonPropertyName("attributeValueId")]
	public int AttributeValueId { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = null!;
}