using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.ReturnedModels
{
    public record BookedAppointment(DateOnly DateTime,string FirstName,string LastName,string Email);
    
}
