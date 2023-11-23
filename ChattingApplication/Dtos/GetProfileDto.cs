using ChattingApplication.Core.Domains;
using ChattingApplication.CustomDataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace ChattingApplication.Dtos
{
    public class GetProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime RegisteredDate { get; set; }
        public bool IsVerified { get; set; }
    }
}
