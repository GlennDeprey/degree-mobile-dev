using Mde.Project.Mobile.Core.Models.Reports;
using Mde.Project.Mobile.Core.ResultModels;

namespace Mde.Project.Mobile.Core.Reports.Service.Interfaces
{
    public interface IReportsService
    {
        Task<PagingResultModel<Report>> TryGetReportListAsync(Guid warehouseId, int pageNumber = 1, int pageSize = 10);
        Task<ResultModel<Report>> TryGetReportByIdAsync(Guid reportId);
    }
}
