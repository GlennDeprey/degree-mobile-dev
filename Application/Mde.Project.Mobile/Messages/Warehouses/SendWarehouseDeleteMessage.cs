using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mde.Project.Mobile.Messages.Products;

namespace Mde.Project.Mobile.Messages.Warehouses
{
    public class SendWarehouseDeleteMessage : SendWarehouseIdentifierMessage
    {
        public string Name { get; set; }

        public SendWarehouseDeleteMessage(Guid id, string name) : base(id)
        {
            Id = id;
            Name = name;
        }
    }
}
