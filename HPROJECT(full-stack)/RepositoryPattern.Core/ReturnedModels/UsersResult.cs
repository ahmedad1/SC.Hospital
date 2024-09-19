
using Newtonsoft.Json;
using RepositoryPatternWithUOW.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.ReturnedModels
{
    public record UsersResult
    {
        public UsersResult(int id,
                           string firstName,
                           string lastName,
                           string userName,
                           string email,
                           string gender,
                           DateOnly birthDate,
                           bool emailConfirmed,
                           string? departmentName,
                           float? price,
                           string? biography

                      )
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Email = email;
            Gender = gender;
            BirthDate = birthDate;
            EmailConfirmed = emailConfirmed;
            DepartmentName = departmentName;
            Price = price;
            Biography = biography;
            
        }

        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string UserName { get; }
        public string Email { get; }
        public string Gender { get; }
        public DateOnly BirthDate { get; }
        public float? Price { get; set; }
        public string? Biography { get; set; }
        public bool EmailConfirmed { get; }
        public string? DepartmentName { get; }

    }
}
