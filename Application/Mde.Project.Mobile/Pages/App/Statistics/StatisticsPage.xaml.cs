using LiveChartsCore.SkiaSharpView.Maui;
using Mde.Project.Mobile.Controls.Base;
using Mde.Project.Mobile.ViewModels.App.Statistics;

namespace Mde.Project.Mobile.Pages.App.Statistics;

public partial class StatisticsPage : SfContentPage
{
	private readonly StatisticsViewModel _statisticsViewModel;
    protected override Type ViewModelType { get; set; } = typeof(StatisticsViewModel);
    public StatisticsPage(StatisticsViewModel statisticsViewModel)
    {
        _statisticsViewModel = statisticsViewModel;
        BindingContext = _statisticsViewModel;
        InitializeComponent();
    }
}