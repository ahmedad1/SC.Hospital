using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.EfCore.MailService
{
    public interface IMailService
    {
        public Task Send(string to, string subject, string body);
    }
}
