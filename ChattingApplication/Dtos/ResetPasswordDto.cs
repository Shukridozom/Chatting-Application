using ChattingApplication.CustomDataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ChattingApplication.Dtos
{
    public class ResetPasswordDto
    {
        public string Code { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(16)]
        [PasswordCharacterSet]
        [PasswordPropertyText]
        public string Password { get; set; }

        [Required]
        [MaxLength(16)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
