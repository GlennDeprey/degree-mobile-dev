namespace Mde.Project.Api.Core.Entities.Warehouses
{
    public class Warehouse : BaseEntity
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Phone { get; set; }
        public Guid LocationInfoId { get; set; }
        public WarehouseLocationInfo LocationInfo { get; set; }
        public Guid? GoogleInfoId { get; set; }
        public WarehouseGoogleInfo GoogleInfo { get; set; }
        public decimal Earnings { get; set; }
        public ICollection<WarehouseItem> WarehouseItems { get; set; }
    }
}
