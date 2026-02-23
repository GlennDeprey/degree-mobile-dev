using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Products;

namespace Mde.Project.Mobile.Pages.App.Products;

public partial class BrandsPage : SfContentPage
{
    private readonly BrandsViewModel _brandViewModel;
    protected override Type ViewModelType { get; set; } = typeof(BrandsViewModel);
    public BrandsPage(BrandsViewModel brandViewModel)
	{
        _brandViewModel = brandViewModel;
        BindingContext = _brandViewModel;
        InitializeComponent();
    }

    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            await _brandViewModel.InitializeBrandsCommand.ExecuteAsync(null);
        }
    }
}