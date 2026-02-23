using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Products;

namespace Mde.Project.Mobile.Pages.App.Products;

public partial class BrandDetailPage : SfContentPage
{
	private readonly BrandDetailViewModel _brandDetailViewModel;
    protected override Type ViewModelType { get; set; } = typeof(BrandDetailViewModel);
    public BrandDetailPage(BrandDetailViewModel brandDetailViewModel)
	{
        _brandDetailViewModel = brandDetailViewModel;
        BindingContext = _brandDetailViewModel;
        InitializeComponent();
	}
}