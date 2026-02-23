using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Warehouses;

namespace Mde.Project.Mobile.Pages.App.Warehouses;

public partial class WarehouseDetailPage : SfContentPage
{
	private readonly WarehouseDetailViewModel _warehouseDetailViewModel;
    protected override Type ViewModelType { get; set; } = typeof(WarehouseDetailViewModel);
    public WarehouseDetailPage(WarehouseDetailViewModel warehousesDetailViewModel)
	{
        _warehouseDetailViewModel = warehousesDetailViewModel;
        BindingContext = _warehouseDetailViewModel;
        InitializeComponent();
	}
}