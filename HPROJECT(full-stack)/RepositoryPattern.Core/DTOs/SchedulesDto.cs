using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.DTOs
{
    public class SchedulesDto
    {
        public string userName { get; set; }
        public List<DateTime>Schedules { get; set; }
    }
}
