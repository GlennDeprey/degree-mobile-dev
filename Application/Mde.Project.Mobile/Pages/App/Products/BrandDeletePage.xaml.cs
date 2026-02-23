using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Products;

namespace Mde.Project.Mobile.Pages.App.Products;

public partial class BrandDeletePage : SfContentPage
{
    private readonly BrandDeleteViewModel _brandDeleteViewModel;
    protected override Type ViewModelType { get; set; } = typeof(BrandDeleteViewModel);
    public BrandDeletePage(BrandDeleteViewModel brandDeleteViewModel)
	{
        _brandDeleteViewModel = brandDeleteViewModel;
        BindingContext = _brandDeleteViewModel;
        InitializeComponent();
	}
}