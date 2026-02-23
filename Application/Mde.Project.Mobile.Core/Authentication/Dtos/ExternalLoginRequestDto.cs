using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Core.Authentication.Dtos
{
    public class ExternalLoginRequestDto
    {
        public string IdToken { get; set; }
        public string Provider { get; set; }
    }
}
