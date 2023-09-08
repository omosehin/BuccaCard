
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Buccacard.Services.NotificationService
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html, string from);
    }
    public class EmailService : IEmailService
    {
        private readonly MailSetting _mailSettings;

        public EmailService(IOptions<MailSetting> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public void Send(string to, string subject, string html, string from)
        {
            SmtpClient client = new SmtpClient();

            client.Host = _mailSettings.SmtpHost ?? "N/A";
            client.Port = _mailSettings.SmtpPort;
            client.EnableSsl = true;

            client.Credentials = new NetworkCredential("Omosehin14@gmail.com", "Lsc1003177");

            MailMessage message = new MailMessage();

            message.From = new MailAddress(from);
            message.To.Add(to);

            message.Subject = subject;
            message.Body = html;

            client.Send(message);
        }
    }
}
