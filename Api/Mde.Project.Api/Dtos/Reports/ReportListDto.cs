using Mde.Project.Api.Dtos.Products;

namespace Mde.Project.Api.Dtos.Reports
{
    public class ReportListDto
    {
        public IEnumerable<ReportDto> Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
