using Microsoft.AspNetCore.Identity;
using RepositoryPatternWithUOW.Core.Enums;
using RepositoryPatternWithUOW.Core.Models;


namespace RepositoryPattern.Core.Models
{
    public abstract class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; }=null!;
        public string? UserName { get; set; }=null!;
        public string Email { get; set; } =null!;
        public string? Password { get; set; } = null!;
        public Gender? Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
     
        public bool EmailConfirmed { get; set; }
        public virtual IdentityTokenVerification IdentityTokenVerification { get; set; } = null!;

        public virtual ICollection<UserConnections> UserConnections { get; set; }=null!;
        public virtual ICollection<Group> Groups { get; set; } = null!;
        public virtual ICollection<RefreshToken> RefreshToken { get; set; } = null!;
        public virtual VerificationCode? VerificationCode { get; set; }




    }
}
