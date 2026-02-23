namespace Mde.Project.Api.Core.Services.RequestModel
{
    public class ReportCreationModel
    {
        public Guid WarehouseId { get; set; }
        public Guid? DestinationWarehouseId { get; set; }
        public Guid ProductId { get; set; }
        public int QuantityChange { get; set; }
        public string Tag { get; set; }
    }
}
