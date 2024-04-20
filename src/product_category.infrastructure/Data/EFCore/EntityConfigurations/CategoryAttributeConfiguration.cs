using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using product_category.core.Domain.EFCore;

namespace product_category.infrastructure.Data.EFCore.EntityConfigurations;

public class CategoryAttributeConfiguration : IEntityTypeConfiguration<CategoryAttribute>
{
	public void Configure(EntityTypeBuilder<CategoryAttribute> builder)
	{
		builder.ToTable("category-attributes");

		builder.HasKey(e => e.Id);
		builder.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnName("id");

		builder.Property(e => e.CategoryId).ValueGeneratedOnAdd().HasColumnName("categoryId");
		builder.Property(e => e.Name).IsRequired().HasMaxLength(maxLength: 100).HasColumnName("name");
		builder.Property(e => e.Varianter).IsRequired().HasColumnName("varianter");
		builder.Property(e => e.Grouped).IsRequired(false).HasColumnName("grouped");

		builder.HasMany(e => e.CategoryAttributeValues).WithOne().HasForeignKey(e => e.CategoryAttributeId);
	}
}
