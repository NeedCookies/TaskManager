using System.Net.Mail;
using TaskManagerApi.Abstractions;

namespace TaskManagerApi.Services
{
    public class MessageService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IConfiguration configuration, 
            ILogger<MessageService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendMessage(string email, string message)
        {
            var mail = new MailMessage(email, message);
            using (var smtp = new SmtpClient(
                _configuration.GetValue<string>("EmailOptions:Host"),
                int.Parse(_configuration.GetValue<string>("EmailOptions:Port"))
                ))
            {
                smtp.Credentials = new System.Net.NetworkCredential(
                    _configuration.GetValue<string>("EmailOptions:MailFrom"),
                    _configuration.GetValue<string>("EmailOptions:Password"));
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mail);
            }
        }
    }
}
