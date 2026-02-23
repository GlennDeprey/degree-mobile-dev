namespace Mde.Project.Api.Dtos.Warehouses
{
    public class WarehouseProductListDto
    {
        public IEnumerable<WarehouseItemDto> Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
