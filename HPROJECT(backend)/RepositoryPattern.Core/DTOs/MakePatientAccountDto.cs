﻿using Newtonsoft.Json.Converters;
using RepositoryPatternWithUOW.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.DTOs
{
    public class MakePatientAccountDto
    {
        [StringLength(100)]

        public string FirstName { get; set; }
        [StringLength(100)]

        public string LastName { get; set; }
        [StringLength(100)]
        public string UserName { get; set; }

        [StringLength(100)]
        [RegularExpression(@"\w+@\w+\.\w+(\.\w+)*", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [StringLength(100)]
        public string Password { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender Gender { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateOnly BirthDate { get; set; }
    }
}
