
using Mde.Project.Mobile.Core.Models.Statistics;
using Mde.Project.Mobile.Core.ResultModels;

namespace Mde.Project.Mobile.Core.Statistics.Interface
{
    public interface IStatisticsService
    {
        Task<CollectionResultModel<WarehouseStats>> TryGetWarehouseStatsAsync(Guid warehouseId);
    }
}
