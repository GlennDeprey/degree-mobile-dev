using Mde.Project.Api.Core.Entities.Reports;
using Mde.Project.Api.Core.Services.Interfaces.Base;
using Mde.Project.Api.Core.Services.Models;
using Mde.Project.Api.Core.Services.RequestModel;

namespace Mde.Project.Api.Core.Services.Interfaces
{
    public interface IReportsService : ICrudService<Report>
    {
        Task<PagingModel<IEnumerable<Report>>> GetReportsOrderedAsync(Guid? warehouseId = null, int pageNumber = 1, int pageSize = 10);
        Task<ResultModel<Report>> CreateReportAsync(ReportCreationModel reportCreation);
    }
}
