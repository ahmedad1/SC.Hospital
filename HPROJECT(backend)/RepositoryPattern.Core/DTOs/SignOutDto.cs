using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.Core.DTOs
{
    public class SignOutDto
    {
        public string UserName { get; set; }
        public string RefreshToken { get; set; }
    }
}
