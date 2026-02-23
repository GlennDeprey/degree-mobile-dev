namespace Mde.Project.Api.Core.Entities.Warehouses
{
    public class WarehouseLocationInfo : BaseEntity
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
