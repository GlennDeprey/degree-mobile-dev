using CommunityToolkit.Maui.Views;
using Mde.Project.Mobile.ViewModels.App.Products;

namespace Mde.Project.Mobile.Pages.App.Products;

public partial class ProductFilterModal : Popup
{
	private readonly ProductsViewModel _productsViewModel;
    public ProductFilterModal(ProductsViewModel productsViewModel)
	{
		_productsViewModel = productsViewModel;
        BindingContext = _productsViewModel;
        InitializeComponent();
    }

    private async void Button_OnPressed(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}