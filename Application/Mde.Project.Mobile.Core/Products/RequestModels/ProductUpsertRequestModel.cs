namespace Mde.Project.Mobile.Core.Products.RequestModels
{
    public class ProductUpsertRequestModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public Guid BrandId { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public decimal SalesPrice { get; set; }
        public Guid SalesTaxId { get; set; }
        public Guid CategoryId { get; set; }
        public string Barcode { get; set; }
    }
}
