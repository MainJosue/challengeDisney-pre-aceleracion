using ChallengeDisney.Entidades;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeDisney.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(Usuario usuario);
    }

    public class SendGridMailService : IMailService
    {
        private readonly ILogger<SendGridMailService> _logger;
        private readonly ISendGridClient _cliente;
        public SendGridMailService(ISendGridClient client, ILogger<SendGridMailService> logger)
        {
            _logger = logger; 
            _cliente = client;
        }
        public async Task SendEmailAsync(Usuario usuario)
        {
            try
            {
                _logger.LogInformation($"Enviando email para el usuario {usuario.UserName}");
                var from = new EmailAddress("name@example.com", "DisneyAlkemyChallenge");
                var subject = "Creaste una cuenta";
                var to = new EmailAddress(usuario.Email, usuario.UserName);
                var plainTextContent = "Gracias por registrarte!" + DateTime.Now;
                var htmlContent = "<strong>Gracias por registrarte</strong>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await _cliente.SendEmailAsync(msg);
            }
            catch (Exception e)
            {
                _logger.LogError("Error al enviar el email", e);

            }

            
        }


    }

}
