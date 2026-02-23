using Mde.Project.Api.Dtos.Products;

namespace Mde.Project.Api.Dtos.Warehouses
{
    public class WarehouseProductDto
    {
        public Guid Id { get; set; }
        public BrandDto Brand { get; set; }
        public CategoryDto Category { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public TaxDto SalesTax { get; set; }
    }
}
