using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.Core.ReturnedModels
{
    public class TokensResult
    {
        public string Jwt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationOfJwt { get; set; }
        public DateTime ExpirationOfRefreshToken { get; set; }
        public bool Success { get; set; }
    }
}
