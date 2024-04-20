using product_category.core.Domain.ES.SeedWork;
using System.Text.Json.Serialization;

namespace product_category.core.Domain.ES;

public class Category : ESEntity
{
	[JsonPropertyName("parentId")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public int? ParentId { get; set; }

	[JsonPropertyName("categoryId")]
	public int CategoryId { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = null!;
}