using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using product_category.core.Domain.EFCore;

namespace product_category.infrastructure.Data.EFCore.EntityConfigurations;

public class CategoryAttributeValueConfiguration : IEntityTypeConfiguration<CategoryAttributeValue>
{
	public void Configure(EntityTypeBuilder<CategoryAttributeValue> builder)
	{
		builder.ToTable("category-attribute-values");

		builder.HasKey(e => e.Id);
		builder.Property(e => e.Id).ValueGeneratedOnAdd().HasColumnName("id");

		builder.Property(e => e.CategoryAttributeId).HasColumnName("categoryAttributeId");
		builder.Property(e => e.Name).IsRequired().HasMaxLength(maxLength: 100).HasColumnName("name");

	}
}
