using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Messages.Authentication
{
    public class SendVerificationMessage : SendEmailMessage
    {
        public string Code { get; set; }
        public SendVerificationMessage(string email, string code) : base(email)
        {
            Code = code;
        }
    }
}
