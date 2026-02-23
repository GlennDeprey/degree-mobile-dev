using Mde.Project.Api.Core.Data;
using Mde.Project.Api.Core.Entities.Warehouses;
using Mde.Project.Api.Core.Services.Base;
using Mde.Project.Api.Core.Services.Interfaces.Warehouses;

namespace Mde.Project.Api.Core.Services.Warehouses
{
    public class WarehouseLocationService : CrudService<WarehouseLocationInfo>, IWarehouseLocationService
    {
        private readonly ApplicationDbContext _context;
        public WarehouseLocationService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
