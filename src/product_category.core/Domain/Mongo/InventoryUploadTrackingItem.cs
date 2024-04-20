using MongoDB.Bson;

namespace product_category.core.Domain.Mongo;

public class InventoryUploadTrackingObject
{
	public ICollection<InventoryUploadTrackingItem> Items { get; set; }

	public void AddItem(BsonDocument item, Dictionary<string, string[]>? failureReasons = null, IEnumerable<int>? identifiers = null)
	{
		Items ??= new HashSet<InventoryUploadTrackingItem>();
		Items.Add(new InventoryUploadTrackingItem(item, failureReasons, identifiers));
	}
}

public class InventoryUploadTrackingItem
{
	public InventoryUploadTrackingItem(BsonDocument item, Dictionary<string, string[]>? failureReasons, IEnumerable<int>? identifiers)
	{
		Item = item;
		Status = failureReasons == null || !failureReasons.Any();
		FailureReasons = failureReasons ?? new Dictionary<string, string[]>();
		Identifiers = identifiers ?? new HashSet<int>();
	}

	/// <summary>
	/// Item in the main list.
	/// </summary>
	public BsonDocument Item { get; } = null!;
	public IEnumerable<int> Identifiers { get; }
	/// <summary>
	/// RBDMS main Id.
	/// </summary>
	public bool Status { get; }
	public Dictionary<string, string[]> FailureReasons { get; }
}