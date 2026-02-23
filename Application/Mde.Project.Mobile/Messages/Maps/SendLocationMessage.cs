using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Messages.Maps
{
    public class SendLocationMessage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public SendLocationMessage(string name, string address, double latitude, double longitude, string id = "")
        {
            Id = id;
            Name = name;
            Address = address;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
