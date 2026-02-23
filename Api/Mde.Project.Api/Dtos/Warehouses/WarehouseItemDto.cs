namespace Mde.Project.Api.Dtos.Warehouses
{
    public class WarehouseItemDto
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public WarehouseProductDto Product { get; set; }
        public int Quantity { get; set; }
        public int MinimumQuantity { get; set; }
        public int RefillQuantity { get; set; }
        public bool HasAutoRefill { get; set; }
    }
}
