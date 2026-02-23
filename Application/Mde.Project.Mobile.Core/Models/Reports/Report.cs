namespace Mde.Project.Mobile.Core.Models.Reports
{
    public class Report : BaseModel
    {
        public Guid WarehouseId { get; set; }
        public Guid ProductId { get; set; }
        public int QuantityChange { get; set; }
        public string Tag { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
