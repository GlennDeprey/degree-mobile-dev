namespace Mde.Project.Api.Dtos.Warehouses
{
    public class WarehouseUpdateDto : WarehouseCreationDto
    {
        public Guid Id { get; set; }
        public ICollection<WarehouseItemDto> Items { get; set; }
    }
}
