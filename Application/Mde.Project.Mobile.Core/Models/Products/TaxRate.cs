using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Core.Models.Products
{
    public class TaxRate : BaseModel
    {
        public string Name { get; set; }

        [JsonPropertyName("TaxRate")]
        public double Rate { get; set; }
    }
}
