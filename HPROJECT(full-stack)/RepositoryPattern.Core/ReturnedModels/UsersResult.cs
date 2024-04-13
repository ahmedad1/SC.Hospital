
using RepositoryPatternWithUOW.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.ReturnedModels
{
    public record UsersResult
    {
        public UsersResult(int Id,
                           string FirstName,
                           string LastName,
                           string UserName,
                           string Email,
                           string Gender,
                           DateOnly BirthDate,
                           bool EmailConfirmed,
                           string? DepartmentName
                              )
        {
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.UserName = UserName;
            this.Email = Email;
            this.Gender = Gender;
            this.BirthDate = BirthDate;
            this.EmailConfirmed = EmailConfirmed;
            this.DepartmentName = DepartmentName;
        }

        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string UserName { get; }
        public string Email { get; }
        public string Gender { get; }
        public DateOnly BirthDate { get; }
        public bool EmailConfirmed { get; }
        [JsonIgnore(Condition =JsonIgnoreCondition.WhenWritingNull)]
        public string? DepartmentName { get; }
    }
}
