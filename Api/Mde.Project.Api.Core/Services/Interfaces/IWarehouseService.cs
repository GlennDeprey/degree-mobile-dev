using Mde.Project.Api.Core.Entities.Warehouses;
using Mde.Project.Api.Core.Services.Interfaces.Base;
using Mde.Project.Api.Core.Services.Interfaces.Warehouses;

namespace Mde.Project.Api.Core.Services.Interfaces
{
    public interface IWarehouseService : ICrudService<Warehouse>
    {
        IWarehouseLocationService Locations { get; }
        IWarehouseItemService Items { get; }
        Task<bool> WarehouseExistAsync(Guid warehouseId);
    }
}
