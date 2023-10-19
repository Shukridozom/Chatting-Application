namespace ChattingApplication.Core.Domains
{
    public class ConfirmationCode
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Code { get; set; }
        public DateTime ExpireDate { get; set; }
        public byte Trials { get; set; }

    }
}
