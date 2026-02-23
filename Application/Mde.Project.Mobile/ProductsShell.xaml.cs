using Mde.Project.Mobile.Pages.App.Products;

namespace Mde.Project.Mobile
{
    public partial class ProductsShell : Shell
    {
        public ProductsShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(MauiRoutes.Products, typeof(ProductsPage));
            Routing.RegisterRoute(MauiRoutes.ProductUpsert, typeof(ProductUpsertPage));
            Routing.RegisterRoute(MauiRoutes.ProductDelete, typeof(ProductDeletePage));
            Routing.RegisterRoute(MauiRoutes.ProductDetail, typeof(ProductDetailPage));

            Routing.RegisterRoute(MauiRoutes.Brands, typeof(BrandsPage));
            Routing.RegisterRoute(MauiRoutes.BrandUpsert, typeof(BrandUpsertPage));
            Routing.RegisterRoute(MauiRoutes.BrandDetail, typeof(BrandDetailPage));
            Routing.RegisterRoute(MauiRoutes.BrandDelete, typeof(BrandDeletePage));

            Routing.RegisterRoute(MauiRoutes.Categories, typeof(CategoriesPage));
            Routing.RegisterRoute(MauiRoutes.CategoryUpsert, typeof(CategoryUpsertPage));
            Routing.RegisterRoute(MauiRoutes.CategoryDetail, typeof(CategoryDetailPage));
            Routing.RegisterRoute(MauiRoutes.CategoryDelete, typeof(CategoryDeletePage));
        }
    }
}
