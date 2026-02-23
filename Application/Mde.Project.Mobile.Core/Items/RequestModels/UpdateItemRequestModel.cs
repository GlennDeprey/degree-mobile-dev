namespace Mde.Project.Mobile.Core.Items.RequestModels
{
    public class UpdateItemRequestModel
    {
        public int Delta { get; set; }
        public bool IsChangeSettings { get; set; }
        public int MinimumQuantity { get; set; }
        public int RefillQuantity { get; set; }
        public bool HasAutoRefill { get; set; }
    }
}
