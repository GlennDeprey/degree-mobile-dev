namespace Mde.Project.Mobile.Models.Statistics
{
    public class LatestSaleInfoModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int Sales { get; set; }
        public int Restock { get; set; }
    }
}
