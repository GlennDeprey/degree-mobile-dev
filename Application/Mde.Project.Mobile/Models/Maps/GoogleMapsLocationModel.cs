using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Models.Maps
{
    public class GoogleMapsLocationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public Location Location { get; set; }
    }
}
