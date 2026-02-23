using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Operations;

namespace Mde.Project.Mobile.Pages.App.Operations;

public partial class StockTransferPage : SfContentPage
{
    private readonly StockTransferViewModel _viewModel;
    protected override Type ViewModelType { get; set; } = typeof(StockTransferViewModel);
    public StockTransferPage(StockTransferViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void AutoCompleteTextField_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        await _viewModel.SearchProductsCommand.ExecuteAsync(e.NewTextValue);
    }
}