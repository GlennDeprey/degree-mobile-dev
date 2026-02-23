using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Api.Core.Services.RequestModel
{
    public class WarehouseStatsCreationModel
    {
        public Guid WarehouseId { get; set; }
        public int TotalSales { get; set; }
        public int TotalRestock { get; set; }
    }
}
