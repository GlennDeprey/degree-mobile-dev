namespace Mde.Project.Api.Dtos.Products
{
    public class BrandListDto
    {
        public IEnumerable<BrandDto> Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
