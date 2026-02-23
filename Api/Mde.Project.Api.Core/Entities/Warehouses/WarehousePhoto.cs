using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Api.Core.Entities.Warehouses
{
    public class WarehousePhoto : BaseEntity
    {
        public Guid GoogleInfoId { get; set; }
        public string PhotoUri { get; set; }
    }
}
