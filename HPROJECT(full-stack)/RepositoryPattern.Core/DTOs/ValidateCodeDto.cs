using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.DTOs
{
    public class ValidateCodeDto
    {
        [StringLength(100)]
        [RegularExpression(@"\w+@\w+\.\w+(\.\w+)*", ErrorMessage = "Invalid Email")]
        public required string Email { get; set; }
        [StringLength(10)]
        [RegularExpression(@"\d+",ErrorMessage ="Invalid Code")]
        public required string Code { get; set; }
        public bool? Reset { get; set; }
    }
}
