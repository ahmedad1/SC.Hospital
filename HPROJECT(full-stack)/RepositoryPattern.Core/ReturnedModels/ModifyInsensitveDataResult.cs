using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.ReturnedModels
{
    public class ModifyInsensitveDataResult
    {
        public bool Success { get; set; }
        public bool HasRepeatedUserName { get; set; } = false;
    }
}
