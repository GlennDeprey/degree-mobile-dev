using System.ComponentModel.DataAnnotations;

namespace Mde.Project.Api.Dtos.Accounts
{
    public class ForgotPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
