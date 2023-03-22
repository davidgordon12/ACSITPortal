using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace ACSITPortal.Helpers
{
    public class Mailer
    {
        private readonly IConfiguration _configuration;
        private SmtpClient smtpClient;
        public Mailer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureMailSettings()
        {
            // Configure SMTP client
            smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_configuration.GetSection("SMTP")["SMTPEmail"], _configuration.GetSection("SMTP")["SMTPPassword"]);
        }
        public void SendEmail(string toAddress, string subject, string body)
        {
            ConfigureMailSettings();

            // Configure email message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_configuration.GetSection("SMTP")["SMTPEmail"]);
            mailMessage.To.Add(toAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = body;

            // Send email
            smtpClient.Send(mailMessage);
        }
    }
}
