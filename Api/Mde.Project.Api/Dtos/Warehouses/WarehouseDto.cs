using Mde.Project.Api.Core.Entities.Warehouses;

namespace Mde.Project.Api.Dtos.Warehouses
{
    public class WarehouseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Phone { get; set; }
        public WarehouseLocationDto Location { get; set; }
        public WarehouseGoogleDto GoogleInfo { get; set; }
        public ICollection<WarehouseItemDto> Items { get; set; }
        public decimal Earnings { get; set; }
    }
}
