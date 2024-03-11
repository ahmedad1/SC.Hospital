using RepositoryPatternWithUOW.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.ReturnedModels
{
    public class SignUpResult
    {
        public bool  Success  { get; set; }
        public string? AlreadyExistField { get; set; }
    }
}
