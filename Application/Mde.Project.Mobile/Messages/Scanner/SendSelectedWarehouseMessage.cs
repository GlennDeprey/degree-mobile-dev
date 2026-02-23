using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Messages.Scanner
{
    public class SendSelectedWarehouseMessage
    {
        public Guid WarehouseId { get; set; }
        public SendSelectedWarehouseMessage(Guid warehouseId)
        {
            WarehouseId = warehouseId;
        }
    }
}
