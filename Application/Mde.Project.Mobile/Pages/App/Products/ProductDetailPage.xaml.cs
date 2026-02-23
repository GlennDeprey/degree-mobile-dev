using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Products;

namespace Mde.Project.Mobile.Pages.App.Products;

public partial class ProductDetailPage : SfContentPage
{
	private readonly ProductDetailViewModel _productDetailViewModel;
    protected override Type ViewModelType { get; set; } = typeof(ProductDetailViewModel);
    public ProductDetailPage(ProductDetailViewModel productDetailViewModel)
	{
        _productDetailViewModel = productDetailViewModel;
        BindingContext = _productDetailViewModel;
        InitializeComponent();
    }
}