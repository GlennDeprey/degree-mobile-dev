using Mde.Project.Mobile.Core.Models.Products;

namespace Mde.Project.Mobile.Core.Models.Scanner
{
    public class ProductScan : BaseModel
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public string ImageSource { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Barcode { get; set; }
    }
}
