namespace Mde.Project.Api.Dtos.Warehouses
{
    public class WarehouseCreationDto
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Phone { get; set; }
        public WarehouseLocationDto Location { get; set; }
        public WarehouseGoogleDto GoogleInfo { get; set; }
    }
}
