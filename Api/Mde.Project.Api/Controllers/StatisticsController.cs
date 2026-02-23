using Mde.Project.Api.Core.Services.Interfaces;
using Mde.Project.Api.Core.Services.RequestModel;
using Mde.Project.Api.Dtos.Reports;
using Mde.Project.Api.Dtos.Statistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mde.Project.Api.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    [Authorize]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;
        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        public async Task<ActionResult<WarehouseSalesListDto>> GetLatestWarehouseStatsAsync([FromQuery] Guid? warehouseId = null)
        {
            var report = await _statisticsService.GetLatestWarehouseStatsAsync(warehouseId);
            if (report.Success)
            {
                var salesListDto = new WarehouseSalesListDto
                {
                    Items = report.Data.Select(r => new WarehouseSaleDto
                    {
                        Id = r.Id,
                        WarehouseId = r.WarehouseId,
                        TotalSales = r.TotalSales,
                        TotalRestock = r.TotalRestock,
                        CreatedOn = r.CreatedOn,
                    }).ToList(),
                };
                return Ok(salesListDto);
            }

            return BadRequest(report.Errors[0]);
        }

        [HttpGet("{warehouseStatsId}", Name = "GetWarehouseStatsById")]
        public async Task<ActionResult<ReportDto>> GetWarehouseStatsByIdAsync(Guid warehouseStatsId)
        {
            var report = await _statisticsService.GetByIdAsync(warehouseStatsId);
            if (report.Success)
            {
                var saleDto = new WarehouseSaleDto
                {
                    Id = report.Data.Id,
                    WarehouseId = report.Data.WarehouseId,
                    TotalSales = report.Data.TotalSales,
                    TotalRestock = report.Data.TotalRestock,
                    CreatedOn = report.Data.CreatedOn,
                };
                return Ok(saleDto);
            }
            return BadRequest(report.Errors[0]);
        }

        [HttpPost]
        public async Task<ActionResult<WarehouseSaleDto>> CreateWarehouseStatsAsync(WarehouseSaleCreationDto warehouseSaleCreation)
        {
            var requestModel = new WarehouseStatsCreationModel
            {
                WarehouseId = warehouseSaleCreation.WarehouseId,
                TotalSales = warehouseSaleCreation.TotalSales,
                TotalRestock = warehouseSaleCreation.TotalRestock,
            };

            var result = await _statisticsService.CreateWarehouseStatsAsync(requestModel);
            if (!result.Success)
            {
                return BadRequest(result.Errors[0]);
            }

            var saleDto = new WarehouseSaleDto
            {
                Id = result.Data.Id,
                WarehouseId = result.Data.WarehouseId,
                TotalSales = result.Data.TotalSales,
                TotalRestock = result.Data.TotalRestock,
                CreatedOn = result.Data.CreatedOn,
            };
            return CreatedAtRoute("GetWarehouseStatsById", new { warehouseStatsId = saleDto.Id }, saleDto);
        }

        [HttpPut("{warehouseStatsId}")]
        public async Task<ActionResult<WarehouseSaleDto>> UpdateWarehouseStatsAsync(Guid warehouseStatsId, WarehouseSaleUpdateDto warehouseSaleUpdate)
        {
            var requestModel = new WarehouseStatsUpdateModel
            {
                Id = warehouseStatsId,
                WarehouseId = warehouseSaleUpdate.WarehouseId,
                SalesDelta = warehouseSaleUpdate.SalesDelta,
                RestockDelta = warehouseSaleUpdate.RestockDelta,
            };

            var result = await _statisticsService.UpdateWarehouseStatsAsync(requestModel);
            if (!result.Success)
            {
                return BadRequest(result.Errors[0]);
            }

            return Ok();
        }
    }
}
