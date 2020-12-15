using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using MailKit.Net.Imap;

namespace P3_app_plass.Services
{
    public class EmailSender: IEmailSender
    {
        public string HtmlMessage { get; set; }
        public IConfiguration Configuration { get; protected set; }
        public EmailSender(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// Odeslání emailu
        /// </summary>
        /// <param name="email">emailová adresa příjemce</param>
        /// <param name="subject">předmět mailu</param>
        /// <param name="text">plain textová podoba obsahu</param>
        /// <returns>nic</returns>
        public Task SendEmailAsync(string email, string subject, string text)
        {
            const string GMailAccount = "filipplass@gmail.com";

            var clientSecrets = new ClientSecrets
            {
                ClientId = "54201618851-2ec97ipr0dr71kd33rqcq0a40guoqu34.apps.googleusercontent.com",
                ClientSecret = "zyvgZsXTiC9RVwuHXJIoIGQP"
            };

            var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                DataStore = new FileDataStore("CredentialCacheFolder", false),
                Scopes = new[] { "https://mail.google.com/" },
                ClientSecrets = clientSecrets
            });

            // Note: For a web app, you'll want to use AuthorizationCodeWebApp instead.
            var codeReceiver = new LocalServerCodeReceiver();
            var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);

            var credential = await authCode.AuthorizeAsync(GMailAccount, CancellationToken.None);

            if (credential.Token.IsExpired(SystemClock.Default))
                await credential.RefreshTokenAsync(CancellationToken.None);

            var oauth2 = new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken);

            using (var client = new ImapClient())
            {
                await client.ConnectAsync("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(oauth2);
                await client.DisconnectAsync(true);
            }
            var message = new MimeMessage(); // vytvoření mailové zprávy
            message.From.Add(new MailboxAddress(Configuration["EmailSender:FromName"], Configuration["EmailSender:From"]));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            if (HtmlMessage != "") bodyBuilder.HtmlBody = HtmlMessage; // pokud máme HTML zprávu, tak ji připojíme
            bodyBuilder.TextBody = text;
            bodyBuilder.HtmlBody = text;
            message.Body = bodyBuilder.ToMessageBody();
            Int32.TryParse(Configuration["EmailSender:Port"], out int port); // v konfiguraci je port uveden jako text, potřebujeme ho jako číslo
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true; // "vždyověření" certifikátu :)
                client.Connect(Configuration["EmailSender:Server"], port, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable); // připojení klienta k serveru
                client.Authenticate(Configuration["EmailSender:Username"], Configuration["EmailSender:Password"]);
                client.Send(message); // poslání zprávy
                client.Disconnect(true); // odpojení klienta
                return Task.FromResult(0);
            }
        }
    }
}