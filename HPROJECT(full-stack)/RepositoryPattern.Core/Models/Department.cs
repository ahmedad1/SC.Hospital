using RepositoryPattern.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Models
{
    public class Department
    {
        
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public byte[] BackgroundCardImage { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }

    }
}
