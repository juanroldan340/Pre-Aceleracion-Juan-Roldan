using SendGrid;
using SendGrid.Helpers.Mail;

namespace DisneyAPI.Services
{
    public interface IMailService
    {
        Task SendWelcomeMail(string toEmail);
    }
    public class MailService : IMailService
    {
        private readonly IConfiguration _config;

        public MailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendWelcomeMail(string toEmail)
        {
            var apiKey = _config["SendGridToken"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("admapi07@gmail.com", "DisneyApp Non-Response");
            var subject = "Bienvenido a DisneyApp";
            var to = new EmailAddress(toEmail);
            var plainTextContent = "Gracias por registrarte! Echa un vistazo a los personajes, películas y series a los que podrás: Acceder, modificar y eliminar.";
            var htmlContent = "<div><div><h1>DisneyApp</h1><h2>🧐 Echa un vistazo a los personajes, películas y series a los que podrás:</h2><ul><li><p>👀 Acceder </p></li><li><p>🖊 Modificar </p></li><li><p>❌ Eliminar </p></li></ul>Espero que puedas disfrutar de ésta API.<br/><hr/><br/><u> Hecho con 💻 desde La Pampa </u></div></div>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            await client.SendEmailAsync(msg);
        }
    }
}
