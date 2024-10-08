using RepositoryPatternWithUOW.Core.DTOs;
using RepositoryPatternWithUOW.Core.Models;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EfCore.MapToModel
{
    [Mapper]
    public partial class ScheduleMapper
    {
        public partial Schedule MapToSchedule(ScheduleDto scheduleDto);
    }
}
