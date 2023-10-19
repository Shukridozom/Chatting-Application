namespace ChattingApplication.Core.EmailService
{
    public interface IEmailService
    {
        void SendConfirmationCode(string emailAddress, string name, string code);
    }
}