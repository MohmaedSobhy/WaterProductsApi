using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace WaterProducts.services.emial
{
    public class EmailSender : IEmailSender
    {
        private readonly GmailOptions _gmailOptions;

        public EmailSender(IOptions<GmailOptions> gmailOptions)
        {
            _gmailOptions = gmailOptions.Value;
        }

        public async Task<bool> SendEmailAsync()
        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_gmailOptions.Email),
                Subject = "Mohamed Sobhy",
                Body = "You are doing great"
            };

            mailMessage.To.Add("moyamany255@gmail.com");

             var smtpClient = new SmtpClient();
            smtpClient.Host = _gmailOptions.Host;
            smtpClient.Port =_gmailOptions.Port;
            smtpClient.Credentials = new NetworkCredential(
                _gmailOptions.Email, _gmailOptions.Password);
            smtpClient.EnableSsl = true;

            await smtpClient.SendMailAsync(mailMessage);
            return true;
        }

        
    }
}
