
using RepositoryPatternWithUOW.Core.Enums;
using RepositoryPatternWithUOW.Core.Models;


namespace RepositoryPattern.Core.Models
{
    public class Doctor:User
    {
        public byte[]? ProfilePicture { get; set; }
        public int DepartmentId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public Days DaysOfTheWork { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<DoctorPatient> DoctorPatient { get; set; } 

    }
}
