using CoreIdentity.WebUI.OptionsModels;
using CoreIdentity.WebUI.Services.Abstract;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CoreIdentity.WebUI.Services.Concrete
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetEmailLink, string ToEmail)
        {
            var smtpClient = new SmtpClient();

            smtpClient.Host =_emailSettings.Host;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            smtpClient.EnableSsl = true;


            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_emailSettings.Email);
            mailMessage.To.Add(ToEmail);

            mailMessage
                .Subject = "Şifre Resetleme Linki";
            mailMessage.Body = $"Şifrenizi yenilemek için aşağıda yer alan linke tıklayınız. <p>" +
                $"<a href='{resetEmailLink}'>şifre yenileme linki</a>";
            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
