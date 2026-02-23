namespace Mde.Project.Api.Core.Entities.Products
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public Guid? BrandId { get; set; }
        public Brand Brand { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public decimal SalesPrice { get; set; }
        public Guid? SalesTaxId { get; set; }
        public ProductTax SalesTax { get; set; }
        public Guid? CategoryId { get; set; }
        public ProductCategory Category { get; set; }
        public string Barcode { get; set; }
    }
}
