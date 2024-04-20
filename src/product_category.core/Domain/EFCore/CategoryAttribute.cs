using product_category.core.Domain.EFCore.SeedWork;

namespace product_category.core.Domain.EFCore;

public class CategoryAttribute : EFEntity, ITenancy
{
	public int CategoryId { get; set; }

	public ICollection<CategoryAttributeValue> CategoryAttributeValues { get; set; }

	public string Name { get; set; } = null!;

	public bool Varianter { get; set; }

	public bool? Grouped { get; set; }

    public CategoryAttribute()
    {
		CategoryAttributeValues ??= new HashSet<CategoryAttributeValue>();
	}
}