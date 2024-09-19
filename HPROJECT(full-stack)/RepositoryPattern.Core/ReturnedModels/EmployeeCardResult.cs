using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.ReturnedModels
{
    public record EmployeeCardResult(int Id ,string FirstName, string LastName, string Bio,string Price,string ProfilePicture);
  
}
