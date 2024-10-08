using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using RepositoryPattern.EfCore.OptionPattenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.EfCore.MailService
{
    public class MailService (IOptions<MailOptionsModel> mailOptionsModel): IMailService
    {
        
        public async Task Send(string to, string subject, string body)
        {
            var mimiMessage = new MimeMessage();
            mimiMessage.Subject = subject;
            mimiMessage.From.Add(new MailboxAddress(mailOptionsModel.Value.Name,mailOptionsModel.Value.Email));
            mimiMessage.To.Add(MailboxAddress.Parse(to));
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody=body;
            mimiMessage.Body = bodyBuilder.ToMessageBody();
            using (var smtp=new SmtpClient())
            {
                await smtp.ConnectAsync(mailOptionsModel.Value.Host, mailOptionsModel.Value.Port, SecureSocketOptions.StartTls);
;               await smtp.AuthenticateAsync(mailOptionsModel.Value.Email, mailOptionsModel.Value.Password);
                await smtp.SendAsync(mimiMessage);

            }
          
            
        }
    }
}
