using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.Core.Models
{
    public class DoctorPatient
    {
        //public int Id { get; set; }
        public int DoctorId { get; set; }
        public int  PatientId { get; set; }
        public DateOnly Appointment { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
