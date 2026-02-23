using System.ComponentModel.DataAnnotations;

namespace Mde.Project.Api.Dtos.Accounts
{
    public class VerifyCodeRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
