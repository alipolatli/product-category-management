using product_category.core.Domain.EFCore.SeedWork;

namespace product_category.core.Domain.EFCore;

public class CategoryAttributeValue : EFEntity, ITenancy
{
	public int CategoryAttributeId { get; set; }
	public string Name { get; set; } = null!;
}

