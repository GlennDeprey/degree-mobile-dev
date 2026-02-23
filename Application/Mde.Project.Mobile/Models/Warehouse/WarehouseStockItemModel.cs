using Mde.Project.Mobile.Models.Products;

namespace Mde.Project.Mobile.Models.Warehouse
{
    public class WarehouseStockItemModel
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public ProductItemDisplayModel Product { get; set; }
        public int Quantity { get; set; }
        public int MinimumQuantity { get; set; }
        public int RefillQuantity { get; set; }
        public bool HasAutoRefill { get; set; }
    }
}
