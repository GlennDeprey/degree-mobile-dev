namespace Mde.Project.Api.Dtos.Products
{
    public class CategoryListDto
    {
        public IEnumerable<CategoryDto> Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
