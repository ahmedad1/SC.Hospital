
using RepositoryPatternWithUOW.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepositoryPattern.Core.DTOs
{
    public class SignUpDto
    {
        [StringLength(100)]
        public required string FirstName { get; set; }
        [StringLength(100)]
        public required string LastName { get; set; }
        [StringLength(100)]
        public required string UserName { get; set; }
        public required DateOnly BirthDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required Gender Gender { get; set; }
        [StringLength(100)]
        [RegularExpression(@"\w+@\w+\.\w+(\.\w+)*", ErrorMessage = "Invalid Email")]
        public  required string Email { get; set; }
        [StringLength(100)]
        public required string Password { get; set; }
  
      
    }
}
