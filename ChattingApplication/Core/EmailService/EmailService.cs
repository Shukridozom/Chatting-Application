using System;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MailKit.Security;

namespace ChattingApplication.Core.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendConfirmationCode(string emailAddress, string name, string code)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(_config["Email:SenderName"], _config["Email:SenderEmailAddress"]));
            email.To.Add(new MailboxAddress(name, emailAddress));

            email.Subject = "Verification code";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"Hi + {name}, Your verification code is {code}"
            };


            using (var smtp = new SmtpClient())
            {
                smtp.Connect(_config["Email:SMTP_Server"], Convert.ToInt32(_config["Email:SMTP_Port"]), SecureSocketOptions.StartTls);

                smtp.Authenticate(_config["Email:SenderEmailAddress"], _config["Email:SenderPassword"]);

                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}
