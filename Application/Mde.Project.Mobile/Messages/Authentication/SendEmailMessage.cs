using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Messages.Authentication
{
    public class SendEmailMessage
    {
        public string Email { get; set; }

        public SendEmailMessage(string email)
        {
            Email = email;
        }
    }
}
