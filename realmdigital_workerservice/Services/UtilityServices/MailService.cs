using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using realmdigital_workerservice.Inerfaces;
using realmdigital_workerservice.Models;

namespace realmdigital_workerservice.Services.UtilityServices
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(Mail mail)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            if(string.IsNullOrEmpty(mail.ToEmail)) //If the To email is blank, add the sender email.For this assessment only.
            {
                email.To.Add(MailboxAddress.Parse(_mailSettings.Mail));
            }
            else
            {
                email.To.Add(MailboxAddress.Parse(mail.ToEmail));
            }
            
            email.Subject = mail.Subject;
            var builder = new BodyBuilder();
           
            builder.HtmlBody = mail.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
