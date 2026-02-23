using Mde.Project.Api.Core.Entities.Warehouses;

namespace Mde.Project.Api.Dtos.Warehouses
{
    public class WarehouseStockDto
    {
        public int TotalItems { get; set; }
        public decimal LowestItemPrice { get; set; }
        public decimal HighestItemPrice { get; set; }
        public decimal AverageItemPrice { get; set; }
    }
}
