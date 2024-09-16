using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.ReturnedModels
{
    public record DoctorResult(int Id ,string FirstName ,string LastName, string UserName , string Email , bool EmailConfirmed , string Birthdate, string Gender,string Department);
  
}
