using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Core.Items.RequestModels
{
    public class CreateItemRequestModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
