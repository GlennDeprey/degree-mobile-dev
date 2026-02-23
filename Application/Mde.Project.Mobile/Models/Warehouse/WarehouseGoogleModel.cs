using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Models.Warehouse
{
    public class WarehouseGoogleModel
    {
        public string GoogleAddress { get; set; }
        public string GoogleAddressId { get; set; }
        public ICollection<WarehousePhotoModel> GooglePhotoUris { get; set; }
    }
}
