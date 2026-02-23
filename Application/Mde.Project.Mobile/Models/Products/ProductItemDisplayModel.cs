namespace Mde.Project.Mobile.Models.Products
{
    public class ProductItemDisplayModel
    {
        public Guid Id { get; set; }
        public BrandItemModel Brand { get; set; }
        public string Name { get; set; }
        public string ImageSource { get; set; }
        public CategoryItemModel Category { get; set; }
    }
}
