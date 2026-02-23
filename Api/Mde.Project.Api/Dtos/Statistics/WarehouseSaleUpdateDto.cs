namespace Mde.Project.Api.Dtos.Statistics
{
    public class WarehouseSaleUpdateDto
    {
        public Guid WarehouseId { get; set; }
        public int SalesDelta { get; set; }
        public int RestockDelta { get; set; }
    }
}
  