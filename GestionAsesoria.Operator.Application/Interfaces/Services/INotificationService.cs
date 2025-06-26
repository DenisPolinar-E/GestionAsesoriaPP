using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GestionAsesoria.Operator.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task NotifyByEmailAsync(string toEmail, string subject, string body);
    }

    public class EmailNotificationService : INotificationService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;
        private readonly ILogger<EmailNotificationService> _logger;

        /*public EmailNotificationService(IOptions<NotificationSettings> settings, ILogger<EmailNotificationService> logger)
        {
            _logger = logger;
            var config = settings.Value;

            _fromEmail = config.FromEmail;
            _smtpClient = new SmtpClient(config.SmtpHost, config.SmtpPort)
            {
                Credentials = new NetworkCredential(config.SmtpUser, config.SmtpPassword),
                EnableSsl = config.EnableSsl
            };
        }*/

        public async Task NotifyByEmailAsync(string toEmail, string subject, string body)
        {
            var mail = new MailMessage(_fromEmail, toEmail, subject, body);

            try
            {
                await _smtpClient.SendMailAsync(mail);
                _logger.LogInformation($"Correo enviado a {toEmail}: {subject}");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error al enviar correo a {toEmail}");
            }
        }
    }
}
