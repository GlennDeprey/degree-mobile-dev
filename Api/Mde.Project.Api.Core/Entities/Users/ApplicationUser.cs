using Microsoft.AspNetCore.Identity;

namespace Mde.Project.Api.Core.Entities.Users
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
    }
}
