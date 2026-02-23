using Mde.Project.Mobile.Core.Models.Warehouses;
using Mde.Project.Mobile.Core.ResultModels;
using Mde.Project.Mobile.Core.Warehouses.RequestModels;

namespace Mde.Project.Mobile.Core.Warehouses.Interface;

public interface IWarehouseService
{
    Task<PagingResultModel<Warehouse>> TryGetWarehouseListAsync(string name, int pageNumber);
    Task<ResultModel<Warehouse>> TryGetWarehouseByIdAsync(Guid warehouseId);
    Task<CollectionResultModel<Warehouse>> TryGetWarehouseOptionsAsync();
    Task<BaseResultModel> TryRemoveWarehouseAsync(Guid id);
    Task<BaseResultModel> TryUpsertWarehouseAsync(WarehouseUpsertRequestModel warehouseUpsertRequestModel);
}