using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.ReturnedModels
{
    public class UpdateUserDataResult
    {
        public bool Success { get; set; }
        public bool NewUserNameIsExist { get; set; }
        public bool NewEmailIsExist { get; set; }
    }
}
