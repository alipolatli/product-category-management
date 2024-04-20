namespace product_category.core.Extensions;

public static class LINQExtensions
{
	public static IEnumerable<T> SelectRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>?> selector)
	{
		foreach (var item in source)
		{
			yield return item;

			var children = selector(item);

			if (children != null)
			{
				foreach (var child in children.SelectRecursive(selector))
				{
					yield return child;
				}
			}
		}
	}
}

