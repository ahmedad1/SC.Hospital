using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.Core.DTOs
{
    public class UpdateTokensDto
    {
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
    }
}
