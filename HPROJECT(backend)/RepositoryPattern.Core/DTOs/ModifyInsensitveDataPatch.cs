using Microsoft.AspNetCore.JsonPatch;
using RepositoryPattern.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.DTOs
{
    public class ModifyInsensitveDataPatch
    {
        public JsonPatchDocument<User>  User { get; set; }
        [StringLength(100)]
        public string Password { get; set; }
    }
}
