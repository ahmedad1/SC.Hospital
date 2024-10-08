using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.Core.Models
{
    public class Patient:User
    {
        public virtual ICollection<Doctor>Doctors { get; set; }
        public virtual ICollection<DoctorPatient> DoctorPatient { get; set; } 


    }
}
