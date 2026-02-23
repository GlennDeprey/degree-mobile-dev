using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Products;

namespace Mde.Project.Mobile.Pages.App.Products;

public partial class BrandUpsertPage : SfContentPage
{
	private readonly BrandUpsertViewModel _brandUpsertViewModel;
    protected override Type ViewModelType { get; set; } = typeof(BrandUpsertViewModel);
    public BrandUpsertPage(BrandUpsertViewModel brandUpsertViewModel)
	{
        _brandUpsertViewModel = brandUpsertViewModel;
        BindingContext = _brandUpsertViewModel;
        InitializeComponent();
	}
}