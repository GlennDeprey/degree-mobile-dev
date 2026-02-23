using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Messages.Products
{
    public class SendCategoryDetailMessage : SendCategoryIdentifierMessage
    {
        public string Name { get; set; }
        public SendCategoryDetailMessage(Guid id, string name) : base(id)
        {
            Name = name;
        }
    }
}
