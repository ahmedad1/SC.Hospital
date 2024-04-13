﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RepositoryPattern.EfCore;

#nullable disable

namespace RepositoryPatternWithUOW.EfCore.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.HasSequence("UserSequence");

            modelBuilder.Entity("RepositoryPattern.Core.Models.DoctorPatient", b =>
                {
                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Appointment")
                        .HasColumnType("datetime2");

                    b.HasKey("DoctorId", "PatientId");

                    b.HasIndex("PatientId");

                    b.ToTable("DoctorPatient");
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.Group", b =>
                {
                    b.Property<string>("GroupsName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("GroupsName");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.RefreshToken", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasMaxLength(44)
                        .HasColumnType("varchar");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("UserId", "Token");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR [UserSequence]");

                    SqlServerPropertyBuilderExtensions.UseSequence(b.Property<int>("Id"));

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.UserConnections", b =>
                {
                    b.Property<string>("ConnectionId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ConnectionId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserConnections");
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.UserGroups", b =>
                {
                    b.Property<string>("GroupId")
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.VerificationCode", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "Code");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("VerificationCode");
                });

            modelBuilder.Entity("RepositoryPatternWithUOW.Core.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("BackgroundCardImage")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("RepositoryPatternWithUOW.Core.Models.ScheduleOfDoctor", b =>
                {
                    b.Property<DateTime>("Schedule")
                        .HasColumnType("datetime2");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.HasKey("Schedule", "DoctorId");

                    b.HasIndex("DoctorId");

                    b.ToTable("ScheduleOfDoctor");
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.Doctor", b =>
                {
                    b.HasBaseType("RepositoryPattern.Core.Models.User");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<byte[]>("ProfilePicture")
                        .HasColumnType("varbinary(max)");

                    SqlServerPropertyBuilderExtensions.IsSparse(b.Property<byte[]>("ProfilePicture"));

                    b.HasIndex("DepartmentId");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.Patient", b =>
                {
                    b.HasBaseType("RepositoryPattern.Core.Models.User");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("RepositoryPatternWithUOW.Core.Models.Admin", b =>
                {
                    b.HasBaseType("RepositoryPattern.Core.Models.User");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.DoctorPatient", b =>
                {
                    b.HasOne("RepositoryPattern.Core.Models.Doctor", "Doctor")
                        .WithMany("DoctorPatient")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RepositoryPattern.Core.Models.Patient", "Patient")
                        .WithMany("DoctorPatient")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.RefreshToken", b =>
                {
                    b.HasOne("RepositoryPattern.Core.Models.User", "User")
                        .WithMany("RefreshToken")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.UserConnections", b =>
                {
                    b.HasOne("RepositoryPattern.Core.Models.User", "Users")
                        .WithMany("UserConnections")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.UserGroups", b =>
                {
                    b.HasOne("RepositoryPattern.Core.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RepositoryPattern.Core.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.VerificationCode", b =>
                {
                    b.HasOne("RepositoryPattern.Core.Models.User", "User")
                        .WithOne("VerificationCode")
                        .HasForeignKey("RepositoryPattern.Core.Models.VerificationCode", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RepositoryPatternWithUOW.Core.Models.ScheduleOfDoctor", b =>
                {
                    b.HasOne("RepositoryPattern.Core.Models.Doctor", "Doctor")
                        .WithMany("SchedualsOfDoctor")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.Doctor", b =>
                {
                    b.HasOne("RepositoryPatternWithUOW.Core.Models.Department", "Department")
                        .WithMany("Doctors")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.User", b =>
                {
                    b.Navigation("RefreshToken");

                    b.Navigation("UserConnections");

                    b.Navigation("VerificationCode");
                });

            modelBuilder.Entity("RepositoryPatternWithUOW.Core.Models.Department", b =>
                {
                    b.Navigation("Doctors");
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.Doctor", b =>
                {
                    b.Navigation("DoctorPatient");

                    b.Navigation("SchedualsOfDoctor");
                });

            modelBuilder.Entity("RepositoryPattern.Core.Models.Patient", b =>
                {
                    b.Navigation("DoctorPatient");
                });
#pragma warning restore 612, 618
        }
    }
}
