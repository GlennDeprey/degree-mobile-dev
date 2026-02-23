using Mde.Project.Api.Dtos.Products;

namespace Mde.Project.Api.Dtos.Warehouses
{
    public class WarehouseListDto
    {
        public IEnumerable<WarehouseDto> Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
