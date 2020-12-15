﻿using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using MailKit.Net.Imap;
using Google.Apis.Util;
using System.Threading;
using MailKit.Security;

namespace P3_app_plass.Services
{
    public class GmailSender : IEmailSender
    {
        public IConfiguration _configuration;

        public GmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Odeslání emailu přes GMail
        /// </summary>
        /// <param name="email">emailová adresa příjemce</param>
        /// <param name="subject">předmět mailu</param>
        /// <param name="text">plain textová podoba obsahu</param>
        /// <returns>nic</returns>
        public async Task SendEmailAsync(string email, string subject, string text)
        {
            var message = new MimeMessage(); // vytvoření mailové zprávy
            message.From.Add(new MailboxAddress(_configuration["GmailSender:FromName"], _configuration["GmailSender:From"]));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = text;
            bodyBuilder.HtmlBody = text;

            message.Body = bodyBuilder.ToMessageBody();

            // GMail OAUTH Flow
            var clientSecrets = new ClientSecrets
            {
                ClientId = _configuration["GmailSender:AppID"],
                ClientSecret = _configuration["GmailSender:AppSecret"]
            };

            var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                DataStore = new FileDataStore("CredentialCacheFolder", false),
                Scopes = new[] { "https://mail.google.com/" },
                ClientSecrets = clientSecrets
            });

            var codeReceiver = new LocalServerCodeReceiver();
            var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);

            var credential = await authCode.AuthorizeAsync(_configuration["GmailSender:AccountID"], CancellationToken.None);

            if (credential.Token.IsExpired(SystemClock.Default))
                await credential.RefreshTokenAsync(CancellationToken.None);

            var oauth2 = new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken);

            if (Int32.TryParse(_configuration["GmailSender:Port"], out int port) == false) port = 0; // v konfiguraci je port uveden jako text, potřebujeme ho jako číslo
            using (var client = new SmtpClient()) // vytvoření SMTP klienta
            {
                await client.ConnectAsync(_configuration["GmailSender:Server"], port, SecureSocketOptions.StartTlsWhenAvailable);
                await client.AuthenticateAsync(oauth2);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}