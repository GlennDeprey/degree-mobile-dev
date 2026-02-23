
namespace Mde.Project.Api.Core.Services.RequestModel
{
    public class WarehouseStatsUpdateModel
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public int SalesDelta { get; set; }
        public int RestockDelta { get; set; }
    }
}
