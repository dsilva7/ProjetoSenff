using SendGrid;
using SendGrid.Helpers.Mail;

namespace Common
{
    public class EmailService
    {
        // readonly ISendGridClient _sendGridClient;
        private const string EMAIL = "dwsilva1996@gmail.com";
        private const string EMPRESA = "Senff";

        //public EmailService(ISendGridClient sendGridClient)
        //{
        //    _sendGridClient = sendGridClient;
        //}

        public async Task<bool> EnviarEmailAsync(string destinatario, string assunto, string corpo)
        {
            var apiKey = Environment.GetEnvironmentVariable("ApiKey");
            var client = new SendGridClient(apiKey);
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
