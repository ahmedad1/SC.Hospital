using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Enums
{
    [Flags]
    public enum Days:byte
    { 
        Sat=1,
        Sun=2,
        Mon=4,
        Tue=8,
        Wed=16,
        Thu=32,
        Fri=64
    }
}
