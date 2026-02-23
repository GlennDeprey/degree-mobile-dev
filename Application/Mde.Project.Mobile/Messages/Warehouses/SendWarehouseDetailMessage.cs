using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mde.Project.Mobile.Models.Warehouse;

namespace Mde.Project.Mobile.Messages.Warehouses
{
    public class SendWarehouseDetailMessage
    {
        public WarehouseDetailModel Warehouse { get; set; }
        public SendWarehouseDetailMessage(WarehouseDetailModel warehouse)
        {
            Warehouse = warehouse;
        }
    }
}
