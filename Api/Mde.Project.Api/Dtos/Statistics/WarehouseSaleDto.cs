namespace Mde.Project.Api.Dtos.Statistics
{
    public class WarehouseSaleDto : BaseDto
    {
        public Guid WarehouseId { get; set; }
        public int TotalSales { get; set; }
        public int TotalRestock { get; set; }
    }
}
