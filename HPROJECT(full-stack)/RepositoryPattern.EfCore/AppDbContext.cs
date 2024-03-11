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
        public DbSet<Admin>Admin { get; set; }
        
        
        public AppDbContext(DbContextOptions options):base(options) 
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
;           
            builder.Entity<Group>().HasKey(x => x.GroupsName);
            builder.Entity<Group>().Property(x => x.GroupsName).HasMaxLength(255);
            
            builder.Entity<Doctor>().HasMany(x => x.Patients).WithMany(x => x.Doctors).UsingEntity<DoctorPatient>();
            builder.Entity<Doctor>().HasMany(x => x.SchedualsOfDoctor).WithOne(x => x.Doctor).HasForeignKey(x => x.DoctorId);
            builder.Entity<Doctor>().HasOne(x => x.Department).WithMany(x => x.Doctors).HasForeignKey(x => x.DepartmentId);

           
            builder.Entity<User>().HasMany(x => x.Groups).WithMany(x => x.Users).UsingEntity<UserGroups>();
            builder.Entity<User>().HasOne(x => x.VerificationCode).WithOne(x => x.User).HasForeignKey<VerificationCode>(x => x.UserId);
            builder.Entity<User>().HasDiscriminator<string>("Discriminator").HasValue<Doctor>("Doc").HasValue<Patient>("Pat").HasValue<Admin>("Adm");
            builder.Entity<User>().Property(x=>x.Discriminator).HasMaxLength(3).HasColumnType("varchar");
            builder.Entity<User>().Property(x => x.Gender).HasConversion(x => x.ToString(), x => (Gender)Enum.Parse(typeof(Gender), x));
            builder.Entity<User>().Property(x => x.Email).HasColumnType("varchar").HasMaxLength(100);
            builder.Entity<User>().HasIndex(x => x.Email).IsUnique() ;
            builder.Entity<User>().HasIndex(x => x.UserName).IsUnique() ;
            builder.Entity<User>().Property(x => x.Password).HasMaxLength(100) ;
       
           
            builder.Entity<VerificationCode>().HasKey(x => new { x.UserId, x.Code });
            
           
            builder.Entity<UserConnections>().HasOne(x => x.Users).WithMany(x=>x.UserConnections).HasForeignKey(x=>x.UserId)
                .IsRequired();
            builder.Entity<UserConnections>().HasKey(x =>new { x.ConnectionId,x.UserId});
            builder.Entity<UserConnections>().Property(x => x.ConnectionId).HasMaxLength(255);

         
            builder.Entity<RefreshToken>().HasOne(x => x.User).WithMany(x => x.RefreshToken).HasForeignKey(x=>x.UserId);
            builder.Entity<RefreshToken>().HasKey(x => new { x.UserId,x.Token});
            builder.Entity<RefreshToken>().Property(x => x.Token).HasMaxLength(44).HasColumnType("varchar");


            builder.Entity<ScheduleOfDoctor>().HasKey(x => new {x.Schedule,x.DoctorId});

            






        }
    }
}
