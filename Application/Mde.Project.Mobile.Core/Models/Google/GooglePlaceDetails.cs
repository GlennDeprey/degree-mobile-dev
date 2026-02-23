using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Core.Models.Google
{
    public class GooglePlaceDetails
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public ICollection<GooglePlacePhoto> PhotoUris { get; set; }
    }
}
