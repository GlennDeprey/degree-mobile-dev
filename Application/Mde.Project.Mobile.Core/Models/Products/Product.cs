namespace Mde.Project.Mobile.Core.Models.Products
{
    public class Product : BaseModel
    {
        public string Name { get; set; }
        public Brand Brand { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public TaxRate SalesTax { get; set; }
        public Category Category { get; set; }
        public string Barcode { get; set; }
    }
}
