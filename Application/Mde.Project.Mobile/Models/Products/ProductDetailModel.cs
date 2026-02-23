namespace Mde.Project.Mobile.Models.Products
{
    public class ProductDetailModel
    {
        public Guid? ProductId { get; set; }
        public string Name { get; set; }
        public BrandItemModel Brand { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public decimal SalesPrice { get; set; }
        public TaxItemModel SalesTax { get; set; }
        public CategoryItemModel Category { get; set; }
        public string Barcode { get; set; }
    }
}
