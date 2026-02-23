using Mde.Project.Api.Core.Entities.Reports;
using Mde.Project.Api.Core.Services.Interfaces;
using Mde.Project.Api.Core.Services.RequestModel;
using Mde.Project.Api.Dtos.Reports;
using Microsoft.AspNetCore.Mvc;

namespace Mde.Project.Api.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _reportsService;
        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpGet]
        public async Task<ActionResult<ReportListDto>> GetReportAsync([FromQuery] Guid? warehouseId = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var report = await _reportsService.GetReportsOrderedAsync(warehouseId, pageNumber, pageSize);
            if (report.Success)
            {
                var reportListDto = new ReportListDto
                {
                    Items = report.Data.Select(r => new ReportDto
                    {
                        Id = r.Id,
                        WarehouseId = r.WarehouseId.Value,
                        ProductId = r.ProductId.Value,
                        QuantityChange = r.QuantityChange,
                        Tag = r.Tag,
                        Description = r.Description,
                        CreatedOn = r.CreatedOn,
                    }).ToList(),
                    TotalItems = report.TotalItems,
                    TotalPages = report.TotalPages
                };
                return Ok(reportListDto);
            }

            return BadRequest(report.Errors[0]);
        }

        [HttpGet("{reportId}", Name = "GetReportById")]
        public async Task<ActionResult<ReportDto>> GetReportByIdAsync(Guid reportId)
        {
            var report = await _reportsService.GetByIdAsync(reportId);
            if (report.Success)
            {
                var reportDto = new ReportDto
                {
                    Id = report.Data.Id,
                    WarehouseId = report.Data.WarehouseId.Value,
                    ProductId = report.Data.ProductId.Value,
                    QuantityChange = report.Data.QuantityChange,
                    Tag = report.Data.Tag,
                    Description = report.Data.Description,
                    CreatedOn = report.Data.CreatedOn,
                };
                return Ok(reportDto);
            }
            return BadRequest(report.Errors[0]);
        }

        [HttpPost]
        public async Task<ActionResult<ReportDto>> CreateReportAsync(ReportCreationDto reportCreationDto)
        {
            var reportCreation = new ReportCreationModel
            {
                WarehouseId = reportCreationDto.WarehouseId,
                DestinationWarehouseId = reportCreationDto.DestinationWarehouseId,
                ProductId = reportCreationDto.ProductId,
                QuantityChange = reportCreationDto.QuantityChange
            };


            var result = await _reportsService.CreateReportAsync(reportCreation);
            if (result.Success)
            {
                var reportDto = new ReportDto
                {
                    Id = result.Data.Id,
                    WarehouseId = result.Data.WarehouseId.Value,
                    ProductId = result.Data.ProductId.Value,
                    QuantityChange = result.Data.QuantityChange,
                    Tag = result.Data.Tag,
                    Description = result.Data.Description,
                    CreatedOn = result.Data.CreatedOn,
                };
                return CreatedAtRoute("GetReportById", new { reportId = reportDto.Id }, reportDto);
            }
            else
            {
                return BadRequest(result.Errors[0]);
            }
        }
    }
}
