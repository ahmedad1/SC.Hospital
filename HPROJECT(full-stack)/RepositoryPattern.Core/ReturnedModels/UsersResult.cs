using RepositoryPatternWithUOW.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.ReturnedModels
{
    public record UsersResult(int Id,
                              string FirstName,
                              string LastName,
                              string UserName,
                              string Email,
                              string Gender,
                              DateOnly BirthDate,
                              bool EmailConfirmed
                              );
}
