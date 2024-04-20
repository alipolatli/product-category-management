using MongoDB.Bson;
using product_category.core.Domain.Mongo;

namespace product_category.api.Application.Commands;

public abstract class CommandHandler
{
	protected InventoryUploadTrackingObject InventoryUploadTrackingObject;
	protected CommandHandler()
	{
		InventoryUploadTrackingObject = new InventoryUploadTrackingObject();
	}

	public void AddInventorUploadItem<T>(T request, Dictionary<string, string[]> errors)
	{
		InventoryUploadTrackingObject.AddItem(request.ToBsonDocument(), errors, null);
	}

	public void AddInventorUploadItem<T>(T request, IEnumerable<int> identifiers)
	{
		InventoryUploadTrackingObject.AddItem(request.ToBsonDocument(), null, identifiers);
	}

	public void AddInventorUploadItem<T>(T request, int identifier)
	{
		InventoryUploadTrackingObject.AddItem(request.ToBsonDocument(), null, new HashSet<int> { identifier });
	}

}