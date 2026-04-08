using Fixawy.EmailSender.Interface;
using System.Net;
using System.Net.Mail;

namespace Fixawy.EmailSender
{
    public class EmailSender : IEmailSeder
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration configuration,ILogger<EmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                using var smtp = new SmtpClient
                {
                    Host = _configuration["EmailSettings:Host"]!,
                    Port = int.Parse(_configuration["EmailSettings:Port"]!),
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_configuration["EmailSettings:Email"],
               _configuration["EmailSettings:Password"])
                };

                using var message = new MailMessage
                {
                    From = new MailAddress(_configuration["EmailSettings:Email"]!, "Pos cashier"),
                    Body = htmlMessage,
                    Subject = subject,
                    IsBodyHtml = true
                };

                message.To.Add(email);

                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error message {ex.Message}");
            }
           
        }
    }
}
