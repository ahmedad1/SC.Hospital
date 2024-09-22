using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternUOW.Core.DTOs
{
    public class GoogleOAuthUserDataDto
    {
        public string Email { get; set; }
        public bool Email_Verified { get; set; }
        public string Name  { get; set; }
        // https://www.googleapis.com/oauth2/v3/userinfo 
        //Authorization header :bearer {token}
    }
}
