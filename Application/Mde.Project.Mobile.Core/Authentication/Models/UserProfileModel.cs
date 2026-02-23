using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Core.Authentication.Models
{
    public class UserProfileModel
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<string> Roles { get; set; }
        public IList<UserLoginInfo> ExternalAccounts { get; set; }
    }
}
