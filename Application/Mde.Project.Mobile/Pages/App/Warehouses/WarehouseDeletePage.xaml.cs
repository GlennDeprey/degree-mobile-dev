using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Warehouses;

namespace Mde.Project.Mobile.Pages.App.Warehouses;

public partial class WarehouseDeletePage : SfContentPage
{
	private readonly WarehouseDeleteViewModel _warehouseDeleteViewModel;
    protected override Type ViewModelType { get; set; } = typeof(WarehouseDeleteViewModel);
    public WarehouseDeletePage(WarehouseDeleteViewModel warehouseDeleteViewModel)
	{
        _warehouseDeleteViewModel = warehouseDeleteViewModel;
        BindingContext = _warehouseDeleteViewModel;
        InitializeComponent();
	}
}