using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.ReturnedModels
{
    public record ScheduleResult(int ShiftId,DayOfWeek day,TimeSpan StartTime,TimeSpan EndTime);
    
}
