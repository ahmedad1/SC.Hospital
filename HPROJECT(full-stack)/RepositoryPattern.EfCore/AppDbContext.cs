using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using RepositoryPattern.Core.Models;
using RepositoryPatternWithUOW.Core.Enums;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.EfCore
{
    public class AppDbContext:DbContext
    {
        
        public DbSet<User> Users { get; set; }  
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Admin>Admin { get; set; }
        
        
        public AppDbContext(DbContextOptions options):base(options) 
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().UseTpcMappingStrategy();
            ;
            builder.Entity<Group>().HasKey(x => x.GroupsName);
            builder.Entity<Group>().Property(x => x.GroupsName).HasMaxLength(255);

            builder.Entity<Doctor>().HasMany(x => x.Patients).WithMany(x => x.Doctors).UsingEntity<DoctorPatient>();
            builder.Entity<Doctor>().Property(x => x.ProfilePicture).IsSparse();
            builder.Entity<Doctor>().Property(x => x.Department).HasConversion(x => x.ToString(), x => (Department)Enum.Parse(typeof(Department),x)).HasMaxLength(20);
            builder.Entity<Doctor>().HasMany(x => x.Schedules).WithOne(x => x.Doctor).HasForeignKey(x => x.DoctorId);
            builder.Entity<Doctor>().Property(x => x.Biography).HasMaxLength(200);
   
            builder.Entity<User>().HasMany(x => x.Groups).WithMany(x => x.Users).UsingEntity<UserGroups>();
            builder.Entity<User>().HasOne(x => x.VerificationCode).WithOne(x => x.User).HasForeignKey<VerificationCode>(x => x.UserId);
            builder.Entity<User>().Property(x => x.FirstName).HasMaxLength(100);
            builder.Entity<User>().Property(x => x.LastName).HasMaxLength(100);
            builder.Entity<User>().Property(x => x.UserName).HasMaxLength(100);
            builder.Entity<User>().Property(x => x.Gender).HasMaxLength(6);
            builder.Entity<User>().Property(x => x.Gender).HasConversion(x => x.ToString(), x => (Gender)Enum.Parse(typeof(Gender), x));
            builder.Entity<User>().Property(x => x.Email).HasColumnType("varchar").HasMaxLength(100);
            builder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            builder.Entity<User>().HasIndex(x => x.UserName).IsUnique();
            builder.Entity<User>().Property(x => x.Password).HasMaxLength(100);
            builder.Entity<Schedule>().HasIndex(x => new { x.DoctorId, x.Day }).IsUnique();

            builder.Entity<VerificationCode>().HasKey(x => new { x.UserId, x.Code });

            builder.Entity<UserConnections>().HasOne(x => x.Users).WithMany(x => x.UserConnections).HasForeignKey(x => x.UserId)
                .IsRequired();
            builder.Entity<UserConnections>().HasKey(x => new { x.ConnectionId, x.UserId });
            builder.Entity<UserConnections>().Property(x => x.ConnectionId).HasMaxLength(255);


            builder.Entity<RefreshToken>().HasOne(x => x.User).WithMany(x => x.RefreshToken).HasForeignKey(x => x.UserId);
            builder.Entity<RefreshToken>().HasKey(x => new { x.UserId, x.Token });
            builder.Entity<RefreshToken>().Property(x => x.Token).HasMaxLength(44).HasColumnType("varchar");

            builder.Entity<IdentityTokenVerification>().HasOne(x => x.User).WithOne(x=>x.IdentityTokenVerification).HasForeignKey<IdentityTokenVerification>(x=>x.UserId);
            builder.Entity<IdentityTokenVerification>().HasKey(x => new {x.UserId, x.Token });
            builder.Entity<IdentityTokenVerification>().Property(x => x.Token).HasMaxLength(44).IsUnicode(false);

            /*
               insert into dbo.Admin (FirstName,LastName,Username,Email,Password,Gender,BirthDate,EmailConfirmed)
               values('Admin','Admin','schospital.admin','admin@schospital.com','password','Male','1989-02-05',1)
            */

            //builder.Entity<Admin>().HasData([new Admin () {BirthDate=DateOnly.FromDateTime(DateTime.Now.AddYears(-25)),Email="schospital.admin@gmail.com",EmailConfirmed=true,FirstName="Admin",Gender=Gender.Male,LastName="Admin",Password=}]);





        }
    }
}
