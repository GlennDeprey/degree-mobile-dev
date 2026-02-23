using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Messages.Products
{
    public class SendCategoryIdentifierMessage
    {
        public Guid Id { get; set; }

        public SendCategoryIdentifierMessage(Guid id)
        {
            Id = id;
        }
    }
}
