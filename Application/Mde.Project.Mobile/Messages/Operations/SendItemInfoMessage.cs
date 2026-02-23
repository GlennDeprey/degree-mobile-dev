using Mde.Project.Mobile.Models.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Messages.Operations
{
    public class SendItemInfoMessage
    {
        public WarehouseStockItemModel Item { get; set; }
        public SendItemInfoMessage(WarehouseStockItemModel item)
        {
            Item = item;
        }
    }
}
