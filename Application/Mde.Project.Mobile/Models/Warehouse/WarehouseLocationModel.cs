namespace Mde.Project.Mobile.Models.Warehouse
{
    public class WarehouseLocationModel
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
