
namespace Mde.Project.Mobile.Models.Warehouse
{
    public class WarehouseStockModel
    {
        public int TotalItems { get; set; }
        public decimal LowestItemPrice { get; set; }
        public decimal HighestItemPrice { get; set; }
        public decimal AverageItemPrice { get; set; }
    }
}
