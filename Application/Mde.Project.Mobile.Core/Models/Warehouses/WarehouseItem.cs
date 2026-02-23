using Mde.Project.Mobile.Core.Models.Products;

namespace Mde.Project.Mobile.Core.Models.Warehouses
{
    public class WarehouseItem
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int MinimumQuantity { get; set; }
        public int RefillQuantity { get; set; }
        public bool HasAutoRefill { get; set; }
    }
}
