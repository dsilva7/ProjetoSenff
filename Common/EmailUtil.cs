using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Common
{
    public class EmailService
    {
        private readonly string _apiKey;
        private const string EMAIL = "dwsilva1996@gmail.com";
        private const string EMPRESA = "Senff";

        public EmailService(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"];
        }

        public async Task<bool> EnviarEmailAsync(string destinatario, string assunto, string corpo)
        {            
            var client = new SendGridClient(_apiKey);
            var emailDeRemetente = new EmailAddress(EMAIL, EMPRESA);
            var emailDestinatario = new EmailAddress(destinatario);
            var mensagem = MailHelper.CreateSingleEmail(emailDeRemetente, emailDestinatario, assunto, corpo, null);

            try
            {
                var resposta = await client.SendEmailAsync(mensagem);
                return resposta.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
                return false;
            }
        }
    }
}
