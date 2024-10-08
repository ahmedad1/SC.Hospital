using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.ReturnedModels
{
    public record EmployeeCardResult(int Id ,string FirstName, string LastName, string? Biography,float Price,string? ProfilePicture,string Gender);
  
}
