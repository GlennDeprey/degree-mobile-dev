using Mde.Project.Mobile.Core.Items.RequestModels;
using Mde.Project.Mobile.Core.Models.Reports;
using Mde.Project.Mobile.Core.Models.Statistics;
using Mde.Project.Mobile.Core.Models.Warehouses;
using Mde.Project.Mobile.Models.Scanner;

namespace Mde.Project.Mobile.Services.Interface
{
    public interface IWarehouseHubService
    {
        Task TryConnectAsync(Guid warehouseId);
        Task TryConnectAsync();
        Task DisconnectAsync();
        void RegisterErrorHandler(Func<string, Task> onErrorHandling);
        void RegisterProductUpdateHandler(Func<Guid, Guid, int, Task> onProductUpdate);
        void RegisterProductAddHandler(Func<Guid, WarehouseItem, Task> onProductAdd);
        void RegisterNewReportHandler(Func<Report, Task> onNewReport);
        void RegisterWarehouseStatsUpdateHandler(Func<WarehouseStats, Task> onWarehouseStatsUpdate);
        void RegisterUpdatedCartHandler(Func<IEnumerable<ScannerProductModel>, Task> onUpdatedCart);
        Task<bool> SendProductUpdateAsync(Guid warehouseId, Guid productId, int quantity, string tag);
        Task<bool> SendProductAddAsync(Guid warehouseId, CreateItemRequestModel createItemRequest, string tag);
        Task<bool> SendStockTransferAsync(Guid sourceWarehouseId, Guid destinationWarehouseId, Guid productId, int quantity);
        Task<bool> SendOrderRequestAsync(Guid warehouseId, IEnumerable<ScannerProductModel> products);
    }
}
