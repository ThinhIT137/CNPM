using System.Net;
using System.Net.Mail;

namespace backend.Servies
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // Lấy cấu hình từ appsettings.json
            var fromEmail = _config["EmailSettings:Email"];
            var password = _config["EmailSettings:Password"];

            var message = new MailMessage(fromEmail, toEmail, subject, body);
            message.IsBodyHtml = true; // Cho phép gửi HTML để email đẹp hơn

            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }
    }
}
