using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace RepositoryPattern.Core.ReturnedModels
{
    public record LoginResult {
        
       public string? Jwt { get;set; }
     
       public string? RefreshToken{get;set;}
        public string Email { get; set; }
        public bool EmailConfirmed{get;set;}
       public bool Success { get; set; }
    }
   
}
