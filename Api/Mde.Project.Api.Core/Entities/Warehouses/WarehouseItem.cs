using Mde.Project.Api.Core.Entities.Products;
namespace Mde.Project.Api.Core.Entities.Warehouses
{
    public class WarehouseItem : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public int Quantity { get; set; }
        public int MinimumQuantity { get; set; }
        public int RefillQuantity { get; set; }
        public bool HasAutoRefill { get; set; }

    }
}
