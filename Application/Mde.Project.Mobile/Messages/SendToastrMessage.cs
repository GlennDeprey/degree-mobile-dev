using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Messages
{
    public class SendToastrMessage
    {
        public string Message { get; set; }
        public Type TargetType { get; set; }

        public SendToastrMessage(string message, Type targetType = null)
        {
            Message = message;
            TargetType = targetType;
        }
    }
}
