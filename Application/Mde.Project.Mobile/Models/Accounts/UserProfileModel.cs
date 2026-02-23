using Mde.Project.Mobile.Core.Authentication.Models;

namespace Mde.Project.Mobile.Models.Accounts
{
    public class UserProfileModel
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<string> Roles { get; set; }
        public IList<ExternalLoginModel> ExternalAccounts { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
