namespace ChattingApplication.Core.Domains
{
    public class User
    {
        public User()
        {
            SentMessages = new List<Message>();
            ReceivedMessages = new List<Message>();
        }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime RegisteredDate { get; set; }
        public bool IsVerified { get; set; }
        public ConfirmationCode ConfirmationCode { get; set; }
        public IList<Message> SentMessages { get; set; }
        public IList<Message> ReceivedMessages { get; set; }

    }
}
