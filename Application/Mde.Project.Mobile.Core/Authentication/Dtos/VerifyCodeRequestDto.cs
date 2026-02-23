using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Core.Authentication.Dtos
{
    public class VerifyCodeRequestDto
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
