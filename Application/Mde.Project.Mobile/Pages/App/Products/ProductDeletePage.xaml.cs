using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Products;

namespace Mde.Project.Mobile.Pages.App.Products;

public partial class ProductDeletePage : SfContentPage
{
    private readonly ProductDeleteViewModel _productDeleteViewModel;
    protected override Type ViewModelType { get; set; } = typeof(ProductDeleteViewModel);
    public ProductDeletePage(ProductDeleteViewModel productDeleteViewModel)
	{
        _productDeleteViewModel = productDeleteViewModel;
        BindingContext = _productDeleteViewModel;
        InitializeComponent();
	}
}