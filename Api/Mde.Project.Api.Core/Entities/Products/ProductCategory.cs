namespace Mde.Project.Api.Core.Entities.Products
{
    public class ProductCategory : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
