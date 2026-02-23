namespace Mde.Project.Api.Dtos.Reports
{
    public class ReportDto : BaseDto
    {
        public Guid WarehouseId { get; set; }
        public Guid ProductId { get; set; }
        public int QuantityChange { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
    }
}
