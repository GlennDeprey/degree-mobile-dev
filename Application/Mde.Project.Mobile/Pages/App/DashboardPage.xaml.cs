using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App;
using SkiaSharp.Extended.UI.Controls;

namespace Mde.Project.Mobile.Pages.App;

public partial class DashboardPage : SfContentPage
{
    private DashboardViewModel _dashboardViewModel;
    protected override Type ViewModelType { get; set; } = typeof(DashboardViewModel);
    public DashboardPage(DashboardViewModel dashboardViewModel)
	{
        _dashboardViewModel = dashboardViewModel;
        BindingContext = _dashboardViewModel;
        InitializeComponent();
    }
}