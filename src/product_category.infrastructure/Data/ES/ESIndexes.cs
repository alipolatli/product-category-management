using product_category.core.Domain.ES;
using product_category.core.Domain.ES.SeedWork;

namespace product_category.infrastructure.Data.ES;

public static class ESIndexes
{
	private static readonly Dictionary<Type, string> IndexNameMap = new Dictionary<Type, string>
			{
				{ typeof(Category), "inventory-categories" },
				{ typeof(CategoryAttribute), "inventory-category-attributes" },
				{ typeof(CategoryAttributeValue), "inventory-category-attribute-values" },
			};

	public static string GetIndexName<T>() where T : ESEntity
	{
		var type = typeof(T);

		if (IndexNameMap.TryGetValue(type, out var indexName))
			return indexName;

		throw new NotSupportedException($"Index name not supported for type {type.Name}");
	}
}