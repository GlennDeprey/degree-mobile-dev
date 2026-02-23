using Mde.Project.Api.Core.Data;
using Mde.Project.Api.Core.Entities.Statistics;
using Mde.Project.Api.Core.Extensions;
using Mde.Project.Api.Core.Services.Base;
using Mde.Project.Api.Core.Services.Interfaces;
using Mde.Project.Api.Core.Services.Models;
using Mde.Project.Api.Core.Services.RequestModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace Mde.Project.Api.Core.Services
{
    public class StatisticsService : CrudService<WarehouseStats>, IStatisticsService
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<WarehouseStats> _dbSet;
        public StatisticsService(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<WarehouseStats>();
        }

        public async Task<ResultModel<IEnumerable<WarehouseStats>>> GetLatestWarehouseStatsAsync(Guid? warehouseId = null)
        {
            var result = new ResultModel<IEnumerable<WarehouseStats>>();
            try
            {
                var query = _dbSet.AsQueryable();

                var sevenDaysAgo = DateTime.Now.AddDays(-6);
                Expression<Func<WarehouseStats, bool>> expression = r => r.CreatedOn >= sevenDaysAgo;

                if (warehouseId.HasValue && warehouseId != Guid.Empty)
                {
                    expression = expression.AndAlso(r => r.WarehouseId == warehouseId.Value);
                }

                query = query.Where(expression);
                result.Data = await query
                             .OrderBy(r => r.CreatedOn).ToListAsync();
            }
            catch
            {
                result.Errors.Add("An error occurred while retrieving the stats.");
            }

            return result;
        }

        public async Task<ResultModel<WarehouseStats>> CreateWarehouseStatsAsync(WarehouseStatsCreationModel warehouseStatsCreation)
        {
            var result = new ResultModel<WarehouseStats>();
            try
            {
                if (warehouseStatsCreation.WarehouseId == Guid.Empty)
                {
                    result.Errors.Add("WarehouseId cannot be empty.");
                    return result;
                }

                if (warehouseStatsCreation.TotalRestock == 0 && warehouseStatsCreation.TotalSales == 0)
                {
                    result.Errors.Add("There are no values changed.");
                    return result;
                }

                var warehouse = await _context.Warehouses.FirstOrDefaultAsync(w => w.Id == warehouseStatsCreation.WarehouseId);
                if (warehouse == null)
                {
                    result.Errors.Add($"Warehouse with ID {warehouseStatsCreation.WarehouseId} not found.");
                    return result;
                }

                var existingItem = _context.WarehouseStats
                    .FirstOrDefault(w => w.WarehouseId == warehouseStatsCreation.WarehouseId && w.CreatedOn.Date == DateTime.Now.Date);
                if (existingItem != null)
                {
                    result.Errors.Add($"There are already statistics, please perform a update.");
                    return result;
                }

                var warehouseStats = new WarehouseStats
                {
                    WarehouseId = warehouseStatsCreation.WarehouseId,
                    TotalRestock = warehouseStatsCreation.TotalRestock,
                    TotalSales = warehouseStatsCreation.TotalSales,
                    CreatedOn = DateTime.Now
                };

                await _context.WarehouseStats.AddAsync(warehouseStats);
                await _context.SaveChangesAsync();

                result.Data = warehouseStats;
            }
            catch
            {
                result.Errors.Add("An error occurred while creating warehouse statistics.");
            }

            return result;
        }

        public async Task<ResultModel<WarehouseStats>> UpdateWarehouseStatsAsync(WarehouseStatsUpdateModel warehouseStatsUpdate)
        {
            var result = new ResultModel<WarehouseStats>();
            try
            {
                var warehouse = await _context.Warehouses.FirstOrDefaultAsync(w => w.Id == warehouseStatsUpdate.WarehouseId);
                if (warehouse == null)
                {
                    result.Errors.Add($"Warehouse with ID {warehouseStatsUpdate.WarehouseId} not found.");
                    return result;
                }

                var warehouseStats = await _context.WarehouseStats.FindAsync(warehouseStatsUpdate.Id);
                if (warehouseStats == null)
                {
                    result.Errors.Add($"Warehouse stats with ID {warehouseStatsUpdate.Id} not found.");
                    return result;
                }

                if (warehouseStatsUpdate.RestockDelta < 0)
                {
                    result.Errors.Add("Restock Delta cannot be negative");
                    return result;
                }

                if (warehouseStatsUpdate.SalesDelta < 0)
                {
                    result.Errors.Add("Sales Delta cannot be negative");
                    return result;
                }

                if (warehouseStatsUpdate.RestockDelta == 0 && warehouseStatsUpdate.SalesDelta == 0)
                {
                    result.Errors.Add("There are no values changed.");
                    return result;
                }


                warehouseStats.TotalRestock += warehouseStatsUpdate.RestockDelta;
                warehouseStats.TotalSales += warehouseStatsUpdate.SalesDelta;

                _context.WarehouseStats.Update(warehouseStats);
                await _context.SaveChangesAsync();
                result.Data = warehouseStats;
            }
            catch
            {
                result.Errors.Add("An error occurred while updating warehouse statistics.");
            }
            return result;
        }
    }
}
