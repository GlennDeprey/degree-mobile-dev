namespace Mde.Project.Mobile.Core.Models.Statistics
{
    public class WarehouseStats : BaseModel
    {
        public Guid WarehouseId { get; set; }
        public int TotalSales { get; set; }
        public int TotalRestock { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
