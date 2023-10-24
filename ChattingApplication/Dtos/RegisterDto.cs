using ChattingApplication.Core.Domains;
using ChattingApplication.CustomDataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace ChattingApplication.Dtos
{
    public class RegisterDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [UsernameCharacterSet]
        public string Username { get; set; }

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

        [Required]
        [MaxLength(255)]
        [NameCharacterSet]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        [NameCharacterSet]
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
