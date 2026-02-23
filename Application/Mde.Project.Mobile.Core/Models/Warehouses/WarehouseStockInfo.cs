using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Core.Models.Warehouses
{
    public class WarehouseStockInfo
    {
        public int TotalItems { get; set; }
        public decimal LowestItemPrice { get; set; }
        public decimal HighestItemPrice { get; set; }
        public decimal AverageItemPrice { get; set; }
    }
}
