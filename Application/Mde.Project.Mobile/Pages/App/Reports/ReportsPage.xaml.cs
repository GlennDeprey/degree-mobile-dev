using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Reports;

namespace Mde.Project.Mobile.Pages.App.Reports;

public partial class ReportsPage : SfContentPage
{
	private readonly ReportsViewModel _reportsViewModel;
    protected override Type ViewModelType { get; set; } = typeof(ReportsViewModel);
    public ReportsPage(ReportsViewModel reportsViewModel)
    {
        _reportsViewModel = reportsViewModel;
        BindingContext = _reportsViewModel;
        InitializeComponent();
    }
}