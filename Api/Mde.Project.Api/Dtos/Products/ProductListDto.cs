namespace Mde.Project.Api.Dtos.Products
{
    public class ProductListDto
    {
        public IEnumerable<ProductListItemDto> Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
