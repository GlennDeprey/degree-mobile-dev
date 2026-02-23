using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Api.Core.Entities.Warehouses
{
    public class WarehouseGoogleInfo : BaseEntity
    {
        public string GoogleAddress { get; set; }
        public string GoogleAddressId { get; set; }
        public ICollection<WarehousePhoto> GooglePhotoUris { get; set; }
    }
}
