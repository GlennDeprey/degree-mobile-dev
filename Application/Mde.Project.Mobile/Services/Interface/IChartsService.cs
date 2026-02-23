using LiveChartsCore.SkiaSharpView.Maui;
using Mde.Project.Mobile.Core.ResultModels;
using Mde.Project.Mobile.Models.Statistics;

namespace Mde.Project.Mobile.Services.Interface
{
    public interface IChartsService
    {
        ResultModel<CartesianChart> GenerateLatestSaleInfo(IEnumerable<LatestSaleInfoModel> latestSalesInfo, bool isDark);
        BaseResultModel UpdateLatestSaleInfo(
            CartesianChart existingChart, LatestSaleInfoModel newEntry);
    }
}
