using ChattingApplication.CustomDataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChattingApplication.Dtos
{
    public class LoginDto
    {
        [Required]
        [MaxLength(50)]
        [UsernameCharacterSet]
        public string Username { get; set; }

        [Required]
        [MaxLength(16)]
        [PasswordCharacterSet]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
