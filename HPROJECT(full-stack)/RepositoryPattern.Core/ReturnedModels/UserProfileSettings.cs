using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.ReturnedModels
{
    public record UserProfileSettings(string FirstName , string LastName, string UserName , string Email , string Role, string Birthdate,string Gender);
   
}
