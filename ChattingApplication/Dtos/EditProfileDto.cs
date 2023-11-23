using ChattingApplication.CustomDataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace ChattingApplication.Dtos
{
    public class EditProfileDto
    {

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
