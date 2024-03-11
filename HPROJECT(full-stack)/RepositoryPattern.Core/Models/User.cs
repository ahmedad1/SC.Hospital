using Microsoft.AspNetCore.Identity;
using RepositoryPatternWithUOW.Core.Enums;


namespace RepositoryPattern.Core.Models
{
    public  class User
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; } 
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Discriminator { get; set; }
        public bool EmailConfirmed { get; set; }
        public virtual ICollection<UserConnections> UserConnections { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<RefreshToken> RefreshToken { get; set; }
        public virtual VerificationCode? VerificationCode { get; set; }




    }
}
