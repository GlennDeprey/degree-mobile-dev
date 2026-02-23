namespace Mde.Project.Api.Dtos.Products
{
    public class ProductCreationDto
    {
        public string Name { get; set; }
        public Guid BrandId { get; set; }
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Guid SalesTaxId { get; set; }
        public Guid CategoryId { get; set; }
    }
}
