namespace Mde.Project.Api.Dtos.Statistics
{
    public class WarehouseSaleCreationDto
    {
        public Guid WarehouseId { get; set; }
        public int TotalSales { get; set; }
        public int TotalRestock { get; set; }
    }
}
