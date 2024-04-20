using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using product_category.core.Domain.EFCore;

namespace product_category.infrastructure.Data.EFCore.EntityConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> builder)
	{
		builder.ToTable("categories");

		builder.HasKey(e => e.Id);
		builder.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnName("id");

		builder.Property(e => e.ParentId).HasColumnName("parentId");
		builder.Property(e => e.Name).IsRequired().HasMaxLength(maxLength: 100).HasColumnName("name");

		builder.HasMany(e => e.SubCategories).WithOne().HasForeignKey(e => e.ParentId);
		builder.HasMany(e => e.CategoryAttributes).WithOne().HasForeignKey(e => e.CategoryId);
	}
}
