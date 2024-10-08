using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.ReturnedModels
{
    public record DoctorDetails(string FirstName,string LastName,float Price , string? Biography,string? ProfilePicture,IEnumerable<ScheduleResult>Shifts,string Gender );
   
}
