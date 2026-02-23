using Mde.Project.Api.Core.Data;
using Mde.Project.Api.Core.Entities.Warehouses;
using Mde.Project.Api.Core.Services.Base;
using Mde.Project.Api.Core.Services.Interfaces;
using Mde.Project.Api.Core.Services.Interfaces.Warehouses;
using Mde.Project.Api.Core.Services.Warehouses;
using Microsoft.EntityFrameworkCore;

namespace Mde.Project.Api.Core.Services
{
    public class WarehouseService : CrudService<Warehouse>, IWarehouseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWarehouseLocationService _warehouseLocationService;
        private readonly IWarehouseItemService _warehouseItemService;
        private readonly DbSet<Warehouse> _dbSet;
        public IWarehouseLocationService Locations => _warehouseLocationService ?? new WarehouseLocationService(_context);
        public IWarehouseItemService Items => _warehouseItemService ?? new WarehouseItemService(_context);

        public async Task<bool> WarehouseExistAsync(Guid warehouseId)
        {
            return await _dbSet.AnyAsync(w => w.Id == warehouseId);
        }

        public WarehouseService(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<Warehouse>();
        }
    }
}
