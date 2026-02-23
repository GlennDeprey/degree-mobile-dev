using CommunityToolkit.Mvvm.ComponentModel;
using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.Models.Google;
using Mde.Project.Mobile.ViewModels.App.Warehouses;

namespace Mde.Project.Mobile.Pages.App.Warehouses;

public partial class WarehouseUpsertPage : SfContentPage
{
    private readonly WarehouseUpsertViewModel _warehouseUpsertViewModel;
    protected override Type ViewModelType { get; set; } = typeof(WarehouseUpsertViewModel);
    public WarehouseUpsertPage(WarehouseUpsertViewModel warehouseUpsertViewModel)
	{
        _warehouseUpsertViewModel = warehouseUpsertViewModel;
        BindingContext = _warehouseUpsertViewModel;
        InitializeComponent();
	}

    private async void AutoCompleteTextField_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        await _warehouseUpsertViewModel.PopulateSuggestionListCommand.ExecuteAsync(e.NewTextValue);
    }
}