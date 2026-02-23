using Mde.Project.Mobile.Models.Products;

namespace Mde.Project.Mobile.Models.Warehouse
{
    public class WarehouseDetailModel : WarehouseDisplayModel
    {
        public WarehouseLocationModel Location { get; set; }
        public WarehouseStockModel Stock { get; set; }
        public WarehouseGoogleModel GoogleInfo { get; set; }
    }
}
