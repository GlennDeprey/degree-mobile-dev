using Mde.Project.Api.Core.Entities.Statistics;
using Mde.Project.Api.Core.Services.Interfaces.Base;
using Mde.Project.Api.Core.Services.Models;
using Mde.Project.Api.Core.Services.RequestModel;

namespace Mde.Project.Api.Core.Services.Interfaces
{
    public interface IStatisticsService : ICrudService<WarehouseStats>
    {
        Task<ResultModel<IEnumerable<WarehouseStats>>> GetLatestWarehouseStatsAsync(Guid? warehouseId = null);
        Task<ResultModel<WarehouseStats>> CreateWarehouseStatsAsync(WarehouseStatsCreationModel warehouseStatsCreation);
        Task<ResultModel<WarehouseStats>> UpdateWarehouseStatsAsync(WarehouseStatsUpdateModel warehouseStatsUpdate);
    }
}
