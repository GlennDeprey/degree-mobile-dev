using System.ComponentModel.DataAnnotations;

namespace Mde.Project.Api.Dtos.Accounts
{
    public class RegisterUserRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 5)]
        public string LastName { get; set; }
    }
}
