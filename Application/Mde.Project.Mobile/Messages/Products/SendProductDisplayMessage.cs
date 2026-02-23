using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Messages.Products
{
    public class SendProductDisplayMessage
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageSource { get; set; }

        public SendProductDisplayMessage()
        {
        }

        public SendProductDisplayMessage(Guid id, string name, string imageSource)
        {
            Id = id;
            Name = name;
            ImageSource = imageSource;
        }
    }
}
