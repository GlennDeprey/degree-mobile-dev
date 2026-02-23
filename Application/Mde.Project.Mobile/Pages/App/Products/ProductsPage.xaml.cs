using CommunityToolkit.Maui.Views;
using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Products;

namespace Mde.Project.Mobile.Pages.App.Products;

public partial class ProductsPage : SfContentPage
{
    private readonly ProductsViewModel _productsViewModel;
    protected override Type ViewModelType { get; set; } = typeof(ProductsViewModel);
    public ProductsPage(ProductsViewModel productsViewModel)
	{
        _productsViewModel = productsViewModel;
        BindingContext = _productsViewModel;
        InitializeComponent();
	}

    private async void Filter_OnPressed(object sender, EventArgs e)
    {
        var popup = new ProductFilterModal(_productsViewModel);
        await Application.Current.MainPage.ShowPopupAsync(popup);
    }

    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            await _productsViewModel.FilterProductsCommand.ExecuteAsync(null);
        }
    }
}