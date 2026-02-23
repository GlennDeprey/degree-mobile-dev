using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Messages.Products
{
    public class SendBrandDetailMessage : SendBrandIdentifierMessage
    {
        public string Name { get; set; }
        public SendBrandDetailMessage(Guid id, string name) : base(id)
        {
            Name = name;
        }
    }
}
