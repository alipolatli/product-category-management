using product_category.core.Abstractions.Services;

namespace product_category.api.Apis;

public class ProductCategoryServices(IInventoryUploadService inventoryUploadService)
{
	public IInventoryUploadService InventoryUploadService { get; set; } = inventoryUploadService;
}