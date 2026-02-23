using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Core.Models.Google
{
    public class GooglePlaceInfo
    {
        public string GoogleAddress { get; set; }
        public string GoogleAddressId { get; set; }
        public ICollection<GooglePlacePhoto> GooglePhotoUris { get; set; }
    }
}
