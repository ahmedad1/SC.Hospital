using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.Core.DTOs
{
    public class LoginDto
    {
        [StringLength(100)]
        public required string UserName { get; set; }
        [StringLength(100)]
        public required string Password { get; set; }
        [StringLength(2000)]
        public string RecaptchaToken { get; set; }
    }
}
