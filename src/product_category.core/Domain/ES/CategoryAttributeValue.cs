using product_category.core.Domain.ES.SeedWork;

namespace product_category.core.Domain.ES;

public class CategoryAttributeValue :ESEntity
{
	public int CategoryAttributeId { get; set; }
	public string Name { get; set; } = null!;
}

