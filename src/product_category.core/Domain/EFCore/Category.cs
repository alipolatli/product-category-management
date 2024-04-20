using product_category.core.Domain.EFCore.SeedWork;

namespace product_category.core.Domain.EFCore;

public class Category : EFEntity, ITenancy
{
	public int? ParentId { get; set; }

	public ICollection<Category> SubCategories { get; set; } 

	public ICollection<CategoryAttribute> CategoryAttributes { get; set; }

	public string Name { get; set; } = null!;

    public Category()
    {
		SubCategories ??= new HashSet<Category>();
		CategoryAttributes ??= new HashSet<CategoryAttribute>();
	}
}