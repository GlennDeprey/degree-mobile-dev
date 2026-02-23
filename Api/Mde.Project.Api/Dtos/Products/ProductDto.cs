
namespace Mde.Project.Api.Dtos.Products
{
    public class ProductDto : BaseDto
    {
        public string Name { get; set; }
        public BrandDto Brand { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public TaxDto SalesTax { get; set; }
        public CategoryDto Category { get; set; }
        public string Barcode { get; set; }
    }
}
