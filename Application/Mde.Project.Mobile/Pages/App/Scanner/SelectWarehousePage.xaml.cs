using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Scanner;

namespace Mde.Project.Mobile.Pages.App.Scanner;

public partial class SelectWarehousePage : SfContentPage
{
    private readonly SelectWarehouseViewModel _selectWarehouseViewModel;
    protected override Type ViewModelType { get; set; } = typeof(SelectWarehouseViewModel);
    public SelectWarehousePage(SelectWarehouseViewModel selectWarehouseViewModel)
    {
        _selectWarehouseViewModel = selectWarehouseViewModel;
        BindingContext = _selectWarehouseViewModel;
        InitializeComponent();
    }
}