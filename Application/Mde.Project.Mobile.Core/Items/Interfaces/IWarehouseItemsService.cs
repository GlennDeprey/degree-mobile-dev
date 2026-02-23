using Mde.Project.Mobile.Core.Items.RequestModels;
using Mde.Project.Mobile.Core.Models.Warehouses;
using Mde.Project.Mobile.Core.ResultModels;

namespace Mde.Project.Mobile.Core.Items.Interfaces
{
    public interface IWarehouseItemsService
    {
        Task<PagingResultModel<WarehouseItem>> TryGetItemsAsync(string name, Guid warehouseId,
            int pageNumber, Guid? brandId = null, Guid? categoryId = null);
        Task<ResultModel<WarehouseItem>> TryGetItemByIdAsync(Guid warehouseId, Guid productId);
        Task<ResultModel<WarehouseItem>> TryGetItemByBarcodeAsync(Guid warehouseId, string barcode, int? currentCount = null);
        Task<ResultModel<IEnumerable<WarehouseItemStock>>> TryGetWarehousesWithProductStockAsync(Guid productId, Guid? excludeWarehouseId, int minQuantity = 1);
        Task<BaseResultModel> TryAddItemAsync(Guid warehouseId, CreateItemRequestModel createItemRequest);
        Task<BaseResultModel> TryUpdateItemAsync(Guid warehouseId, Guid productId, UpdateItemRequestModel updateItemRequest);
        Task<BaseResultModel> TryDeleteItemAsync(Guid warehouseId, Guid productId);
        Task<ResultModel<byte[]>> TryGetWarehouseProductsPdf(Guid warehouseId);
    }
}
