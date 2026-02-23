using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Messages.Products
{
    public class SendProductIdentifierMessage
    {
        public Guid Id { get; set; }

        public SendProductIdentifierMessage(Guid id)
        {
            Id = id;
        }
    }
}
