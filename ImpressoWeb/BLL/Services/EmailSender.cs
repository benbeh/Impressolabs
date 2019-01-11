using System;
using System.Threading.Tasks;
using BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BLL.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string message, IConfiguration configuration)
        {
            var msg = MailHelper.CreateSingleEmail(new EmailAddress(configuration["Data:Email"]), new EmailAddress(email), subject, null, message);

            var client = new SendGridClient(configuration["Data:SENDGRID_API_KEY"]);
            await client.SendEmailAsync(msg);
        }
    }
}