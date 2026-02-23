namespace Mde.Project.Api.Dtos.Warehouses
{
    public class WarehouseItemUpdateDto
    {
        public int Delta { get; set; }
        public bool IsChangeSettings { get; set; }
        public int MinimumQuantity { get; set; }
        public int RefillQuantity { get; set; }
        public bool HasAutoRefill { get; set; }
    }
}
