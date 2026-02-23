using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Operations;

namespace Mde.Project.Mobile.Pages.App.Operations;

public partial class StockSettingsPage : SfContentPage
{
    private readonly StockSettingsViewModel _viewModel;
    protected override Type ViewModelType { get; set; } = typeof(StockSettingsViewModel);
    public StockSettingsPage(StockSettingsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}