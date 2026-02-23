using Mde.Project.Mobile.Core.Models.Google;

namespace Mde.Project.Mobile.Core.Models.Warehouses
{
    public class Warehouse : BaseModel
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Phone { get; set; }
        public WarehouseLocation Location { get; set; }
        public IEnumerable<WarehouseItem> Items { get; set; }
        public WarehouseStockInfo Stock { get; set; }
        public GooglePlaceInfo GoogleInfo { get; set; }
        public decimal Earnings { get; set; }
    }
}
