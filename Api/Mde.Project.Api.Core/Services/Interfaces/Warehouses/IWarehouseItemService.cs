using Mde.Project.Api.Core.Entities.Warehouses;
using Mde.Project.Api.Core.Services.Interfaces.Base;
using Mde.Project.Api.Core.Services.Models;

namespace Mde.Project.Api.Core.Services.Interfaces.Warehouses
{
    public interface IWarehouseItemService : ICrudService<WarehouseItem>
    {
        Task<BaseResult> TryTransferStockAsync(Guid senderWarehouseId, Guid destinationWarehouseId, Guid productId, int quantity);
    }
}
