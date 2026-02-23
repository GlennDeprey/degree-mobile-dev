namespace Mde.Project.Mobile.Models.Reports
{
    public class ReportModel
    {
        public Guid Id { get; set; }
        public Guid WarehouseId { get; set; }
        public Guid ProductId { get; set; }
        public int QuantityChange { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
