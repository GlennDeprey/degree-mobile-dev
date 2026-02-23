using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Warehouses;

namespace Mde.Project.Mobile.Pages.App.Warehouses;

public partial class WarehousesPage : SfContentPage
{
	private readonly WarehousesViewModel _warehousesViewModel;
    protected override Type ViewModelType { get; set; } = typeof(WarehousesViewModel);
    public WarehousesPage(WarehousesViewModel warehousesViewModel)
	{
        _warehousesViewModel = warehousesViewModel;
        BindingContext = _warehousesViewModel;
        InitializeComponent();
	}
    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            await _warehousesViewModel.InitializeWarehousesCommand.ExecuteAsync(null);
        }
    }
}