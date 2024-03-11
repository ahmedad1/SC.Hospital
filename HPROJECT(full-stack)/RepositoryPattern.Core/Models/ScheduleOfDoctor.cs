using RepositoryPattern.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Models
{
    public class ScheduleOfDoctor
    {
        public int DoctorId { get; set; }
        public DateTime Schedule { get; set; }
        public virtual Doctor Doctor { get; set; }

    }

}
