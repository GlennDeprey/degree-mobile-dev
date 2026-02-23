using Microsoft.AspNetCore.Identity;

namespace Mde.Project.Api.Dtos.Accounts
{
    public class UserDto
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
