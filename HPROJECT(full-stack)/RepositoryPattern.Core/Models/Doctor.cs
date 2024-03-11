
using RepositoryPatternWithUOW.Core.Models;


namespace RepositoryPattern.Core.Models
{
    public class Doctor:User
    {
        public byte[] ProfilePicture { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<ScheduleOfDoctor> SchedualsOfDoctor { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<DoctorPatient> DoctorPatient { get; set; } 

    }
}
