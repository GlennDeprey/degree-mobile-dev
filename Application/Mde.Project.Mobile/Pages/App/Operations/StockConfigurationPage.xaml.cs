using CommunityToolkit.Maui.Views;
using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.Pages.App.Warehouses;
using Mde.Project.Mobile.ViewModels.App.Operations;

namespace Mde.Project.Mobile.Pages.App.Operations;

public partial class StockConfigurationPage : SfContentPage
{
	private readonly StockConfigurationViewModel _viewModel;
    public StockConfigurationPage(StockConfigurationViewModel viewModel)
	{
		_viewModel = viewModel;
        BindingContext = _viewModel;
        InitializeComponent();
	}

    protected override Type ViewModelType { get; set; } = typeof(StockConfigurationViewModel);

    private async void Filter_OnPressed(object sender, EventArgs e)
    {
        var popup = new WarehouseProductFilterModal(_viewModel);
        await Application.Current.MainPage.ShowPopupAsync(popup);
    }

    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            await _viewModel.FilterProductsCommand.ExecuteAsync(null);
        }
    }
}