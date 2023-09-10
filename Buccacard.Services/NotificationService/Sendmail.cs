﻿
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Buccacard.Services.NotificationService
{
    public interface IEmailService
    {
        bool Send(string to, string subject, string html, string from);
    }
    public class EmailService : IEmailService
    {
        private readonly MailSetting _mailSettings;

        public EmailService(IOptions<MailSetting> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public bool Send(string to, string subject, string html, string from) //mtn is not work with port 587,25
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_mailSettings.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = html };


            SmtpClient smtp = new SmtpClient();

            smtp.Connect(_mailSettings.SmtpHost, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
            smtp.Send(email);
            smtp.Disconnect(true);
            return true;
        }
    }
}
