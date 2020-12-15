using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Security;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using Google.Apis.Util;

namespace P3_app_plass.Services
{
    public class EmailSender : IEmailSender
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