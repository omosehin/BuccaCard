namespace Buccacard.Services.NotificationService
{
    public class MailSetting
    {
        public string SmtpHost { get; set; } = null;
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; } = null;
        public string SmtpPass { get; set; } = null;
        public string EmailFrom { get; set; } = string.Empty;
    }
}
