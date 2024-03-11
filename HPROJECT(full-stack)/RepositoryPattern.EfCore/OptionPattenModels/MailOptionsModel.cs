using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.EfCore.OptionPattenModels
{
    public class MailOptionsModel
    {

     
        
            public string Host { get; set; }
            public int Port { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        

    }
}
