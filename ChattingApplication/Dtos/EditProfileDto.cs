using ChattingApplication.CustomDataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace ChattingApplication.Dtos
{
    public class EditProfileDto
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
