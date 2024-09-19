
using RepositoryPatternWithUOW.Core.Enums;
using RepositoryPatternWithUOW.Core.Models;


namespace RepositoryPattern.Core.Models
{
    public class Doctor:User
    {
        public string? ProfilePicture { get; set; }
        public Department Department { get; set; }
        public float Price { get; set; }
        public string? Biography{ get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<DoctorPatient> DoctorPatient { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }

    }
}
