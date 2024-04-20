using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace product_category.core.Domain.ES.SeedWork;

public abstract class ESEntity
{
	[Key]
	[JsonPropertyName("_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string Id { get; set; } = null!;
}
