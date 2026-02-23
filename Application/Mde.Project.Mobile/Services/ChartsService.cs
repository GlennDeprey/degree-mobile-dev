using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Maui;
using LiveChartsCore.SkiaSharpView.Painting;
using Mde.Project.Mobile.Core.ResultModels;
using Mde.Project.Mobile.Models.Statistics;
using Mde.Project.Mobile.Services.Interface;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Collections.ObjectModel;
namespace Mde.Project.Mobile.Services
{
    public class ChartsService : IChartsService
    {
        public ChartsService()
        {
        }

        public ResultModel<CartesianChart> GenerateLatestSaleInfo(IEnumerable<LatestSaleInfoModel> latestSalesInfo, bool isDark)
        {
            var result = new ResultModel<CartesianChart>();
            try
            {
                var chart = CreateLatestSalesInfo(latestSalesInfo, isDark);
                result.Data = chart;
            }
            catch (Exception ex)
            {
                result.Message = $"An error occurred while generating the chart: {ex.Message}";
            }

            return result;
        }

        public BaseResultModel UpdateLatestSaleInfo(
            CartesianChart existingChart, LatestSaleInfoModel newEntry)
        {
            var result = new BaseResultModel();
            try
            {
                if (existingChart == null)
                {
                    result.Message = "The existing chart is null.";
                    return result;
                }

                if (newEntry == null)
                {
                    result.Message = "The new entry is null.";
                    return result;
                }

                var salesSeries = existingChart.Series.OfType<ColumnSeries<DateTimePoint>>()
                    .FirstOrDefault(s => s.Name == "Products Sold");
                var restockSeries = existingChart.Series.OfType<ColumnSeries<DateTimePoint>>()
                    .FirstOrDefault(s => s.Name == "Products Restock");

                if (salesSeries == null || restockSeries == null)
                {
                    result.Message = "The chart does not contain the required series.";
                    return result;
                }

                var salesPoint = salesSeries.Values.OfType<DateTimePoint>()
                    .FirstOrDefault(p => p.DateTime.Date == newEntry.Date.Date);

                if (salesPoint != null)
                {
                    salesPoint.Value = newEntry.Sales;
                }
                else
                {
                    var list = salesSeries.Values.ToList();
                    list.Add(new DateTimePoint(newEntry.Date, newEntry.Sales));
                    salesSeries.Values = list;
                }

                // Update or add the restock point
                var restockPoint = restockSeries.Values.OfType<DateTimePoint>()
                    .FirstOrDefault(p => p.DateTime.Date == newEntry.Date.Date);

                if (restockPoint != null)
                {
                    restockPoint.Value = newEntry.Restock;
                }
                else
                {
                    var list = restockSeries.Values.ToList();
                    list.Add(new DateTimePoint(newEntry.Date, newEntry.Restock));
                    restockSeries.Values = list;
                }

                var maxSalesValue = salesSeries.Values.OfType<DateTimePoint>().Max(p => p.Value);
                var maxRestockValue = restockSeries.Values.OfType<DateTimePoint>().Max(p => p.Value);

                var maxYAxis = maxSalesValue > maxRestockValue
                ? maxSalesValue * 1.2 :
                maxRestockValue * 1.2;


                existingChart.YAxes.FirstOrDefault()?.SetLimits(0, maxYAxis.Value);
            }
            catch (Exception ex)
            {
                result.Message = $"An error occurred while updating the chart: {ex.Message}";
            }

            return result;
        }

        private void InitializeDefaultPoints(out ObservableCollection<DateTimePoint> points)
        {
            points = new ObservableCollection<DateTimePoint> 
            {
                new DateTimePoint(DateTime.Now.AddDays(-5).Date, 20),
                new DateTimePoint(DateTime.Now.AddDays(-4).Date, 15),
                new DateTimePoint(DateTime.Now.AddDays(-3).Date, 42),
                new DateTimePoint(DateTime.Now.AddDays(-2).Date, 57),
                new DateTimePoint(DateTime.Now.AddDays(-1).Date, 70),
                new DateTimePoint(DateTime.Now.Date, 0)
            };
        }

        private SKColor GetMauiColor(string resourceKey)
        {
            if (Application.Current.Resources.TryGetValue(resourceKey, out var colorResource) && colorResource is Color mauiColor)
            {
                return mauiColor.ToSKColor();
            }

            return SKColors.Transparent;
        }

        private CartesianChart CreateLatestSalesInfo(IEnumerable<LatestSaleInfoModel> latestSalesInfo, bool isDark)
        {
            latestSalesInfo ??= Enumerable.Empty<LatestSaleInfoModel>();

            InitializeDefaultPoints(out var salesPoints);
            InitializeDefaultPoints(out var restockPoints);

            foreach (var saleInfo in latestSalesInfo)
            {
                var existingSalesPoint = salesPoints.FirstOrDefault(p => p.DateTime.Date == saleInfo.Date.Date);
                if (existingSalesPoint != null)
                {
                    existingSalesPoint.Value = saleInfo.Sales;
                }
                else
                {
                    salesPoints.Add(new DateTimePoint(saleInfo.Date, saleInfo.Sales));
                }

                var existingRestockPoint = restockPoints.FirstOrDefault(p => p.DateTime.Date == saleInfo.Date.Date);
                if (existingRestockPoint != null)
                {
                    existingRestockPoint.Value = saleInfo.Restock;
                }
                else
                {
                    restockPoints.Add(new DateTimePoint(saleInfo.Date, saleInfo.Restock));
                }
            }

            var sales = new ColumnSeries<DateTimePoint>
            {
                Values = salesPoints,
                Name = "Products Sold",
                MaxBarWidth = double.MaxValue,
                IgnoresBarPosition = true,
                Stroke = null,
                Fill = new SolidColorPaint(GetMauiColor("YellowGreen"))
            };

            var maxBarWidth = 60;
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                maxBarWidth = 10;
            }

            var restock = new ColumnSeries<DateTimePoint>
            {
                Values = restockPoints,
                Name = "Products Restock",
                MaxBarWidth = maxBarWidth,
                IgnoresBarPosition = true,
                Stroke = null,
                Fill = new SolidColorPaint(GetMauiColor("Charcoal"))
            };

            var series = new ISeries[] { sales, restock };

            var xAxis = new Axis
            {
                Labeler = value => value.AsDate().ToString("MMMM dd"),
                UnitWidth = TimeSpan.FromDays(1).Ticks,
                MinStep = TimeSpan.FromDays(1).Ticks,
                MaxLimit = DateTime.Now.Ticks,
                Name = "Date",
                NamePaint = isDark ? new SolidColorPaint(GetMauiColor("GhostWhite")) : new SolidColorPaint(GetMauiColor("Cerulean")),
                LabelsPaint = isDark ? new SolidColorPaint(GetMauiColor("GhostWhite")) : new SolidColorPaint(GetMauiColor("Cerulean")),
                Padding = new LiveChartsCore.Drawing.Padding(0,15,0,0),
            };

            var maxYAxis = salesPoints.Max(p => p.Value) > restockPoints.Max(p => p.Value) 
                ? salesPoints.Max(p => p.Value) * 1.2 :
                restockPoints.Max(p => p.Value) * 1.2;

            var yAxis = new Axis
            {
                Name = "Product Count",
                NamePaint = isDark ? new SolidColorPaint(GetMauiColor("GhostWhite")) : new SolidColorPaint(GetMauiColor("Cerulean")),
                LabelsPaint = isDark ? new SolidColorPaint(GetMauiColor("GhostWhite")) : new SolidColorPaint(GetMauiColor("Cerulean")),
                MinLimit = 0,
                MaxLimit = maxYAxis == 0 ? 100 : maxYAxis
            };

            return new CartesianChart
            {
                Series = series,
                XAxes = new[] { xAxis },
                YAxes = new[] { yAxis },
            };
        }
    }
}
