
using Mde.Project.Api.Core.Data;
using Mde.Project.Api.Core.Entities.Reports;
using Mde.Project.Api.Core.Services.Base;
using Mde.Project.Api.Core.Services.Interfaces;
using Mde.Project.Api.Core.Services.Models;
using Mde.Project.Api.Core.Services.RequestModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Mde.Project.Api.Core.Services
{
    public class ReportsService : CrudService<Report>, IReportsService
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Report> _dbSet;
        public ReportsService(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<Report>();
        }

        public async Task<PagingModel<IEnumerable<Report>>> GetReportsOrderedAsync(Guid? warehouseId = null, int pageNumber = 1, int pageSize = 10)
        {
            var result = new PagingModel<IEnumerable<Report>>();
            try 
            {
                if (pageNumber < 1)
                {
                    result.Errors.Add("The page number must be 1 or more.");
                    return result;
                }

                if (pageSize < 1 || pageSize > 100)
                {
                    result.Errors.Add("The page size must be between 1 and 100.");
                    return result;
                }

                var query = _dbSet.AsQueryable();
                Expression<Func<Report, bool>> filter = null;
                if (warehouseId.HasValue && warehouseId != Guid.Empty)
                {
                    query = query.Where(r => r.WarehouseId == warehouseId.Value);
                    filter = r => r.WarehouseId == warehouseId.Value;
                }

                result.Data = await query
                             .OrderByDescending(r => r.CreatedOn)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync();

                result.TotalItems = await GetCountAsync(filter);
                result.TotalPages = (int)Math.Ceiling((double)result.TotalItems / pageSize);
            }
            catch 
            {
                result.Errors.Add("An error occurred while retrieving the reports.");
            }

            return result;
        }

        public async Task<ResultModel<Report>> CreateReportAsync(ReportCreationModel reportCreation)
        {
            var result = new ResultModel<Report>();
            try
            {
                if (reportCreation.ProductId == Guid.Empty)
                {
                    result.Errors.Add("ProductId cannot be empty.");
                    return result;
                }

                if (reportCreation.WarehouseId == Guid.Empty)
                {
                    result.Errors.Add("WarehouseId cannot be empty.");
                    return result;
                }

                if (reportCreation.QuantityChange == 0)
                {
                    result.Errors.Add("QuantityChange cannot be zero.");
                    return result;
                }

                var warehouse = await _context.Warehouses.FirstOrDefaultAsync(w => w.Id == reportCreation.WarehouseId);
                if (warehouse == null)
                {
                    result.Errors.Add($"Warehouse with ID {reportCreation.WarehouseId} not found.");
                    return result;
                }

                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == reportCreation.ProductId);
                if (product == null)
                {
                    result.Errors.Add($"Product with ID {reportCreation.ProductId} not found.");
                    return result;
                }

                var description = reportCreation.QuantityChange > 0
                    ? $"Added {reportCreation.QuantityChange} items of {product.Name}"
                    : $"Removed {Math.Abs(reportCreation.QuantityChange)} items of {product.Name}";

                if (reportCreation.DestinationWarehouseId.HasValue)
                {
                    var destinationWarehouse = await _context.Warehouses.FirstOrDefaultAsync(w => w.Id == reportCreation.DestinationWarehouseId);
                    if (destinationWarehouse != null)
                    {
                        var destinationWarehouseText = reportCreation.QuantityChange > 0 ? $" from {destinationWarehouse.Name}"
                        : $" to {destinationWarehouse.Name}";

                        description += destinationWarehouseText;
                    }      
                }          

                var report = new Report
                {
                    WarehouseId = reportCreation.WarehouseId,
                    ProductId = reportCreation.ProductId,
                    QuantityChange = reportCreation.QuantityChange,
                    Tag = reportCreation.Tag,
                    Description = description,
                    CreatedOn = DateTime.Now
                };

                await _context.Reports.AddAsync(report);
                await _context.SaveChangesAsync();

                result.Data = report;
            }
            catch
            {
                result.Errors.Add("An error occurred while creating the report.");
            }

            return result;
        }
    }
}
