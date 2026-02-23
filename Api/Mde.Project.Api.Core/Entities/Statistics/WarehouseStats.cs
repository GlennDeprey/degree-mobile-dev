using Mde.Project.Api.Core.Entities.Warehouses;

namespace Mde.Project.Api.Core.Entities.Statistics
{
    public class WarehouseStats : BaseEntity
    {
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public int TotalSales { get; set; }
        public int TotalRestock { get; set; }
    }
}
