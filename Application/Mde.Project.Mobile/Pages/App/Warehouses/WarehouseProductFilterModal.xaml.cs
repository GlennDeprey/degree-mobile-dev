using CommunityToolkit.Maui.Views;
using Mde.Project.Mobile.ViewModels.App.Operations;

namespace Mde.Project.Mobile.Pages.App.Warehouses;

public partial class WarehouseProductFilterModal : Popup
{
	private readonly StockConfigurationViewModel _viewModel;
    public WarehouseProductFilterModal(StockConfigurationViewModel filterViewModel)
	{
		_viewModel = filterViewModel;
        BindingContext = _viewModel;
        InitializeComponent();
    }

    private async void Button_OnPressed(object sender, EventArgs e)
    {
        await CloseAsync();
    }
}