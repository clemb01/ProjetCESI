using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Protocols;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier.Main
{
    class EmailMetier : IEmailMetier
    {
        public EmailMetier()
        {
        }

        public async Task SendEmailAsync(string destinataire, string sujet, string message)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("teamfishon67@gmail.com", "Team FishOn");
            var subject = sujet;
            var to = new EmailAddress(destinataire, "Receive user");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

        }

        public Task SendEmailAsync()
        {
            throw new NotImplementedException();
        }
    }
}
