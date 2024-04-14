using Microsoft.AspNetCore.Http;
using RepositoryPattern.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.DTOs
{
    public class DepartmentDto
    {
   
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public IFormFile BackgroundCardImage { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
