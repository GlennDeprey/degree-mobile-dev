namespace Mde.Project.Api.Dtos.Products
{
    public class ProductListItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public BrandDto Brand { get; set; }
        public string Image { get; set; }
        public CategoryDto Category { get; set; }
    }
}
