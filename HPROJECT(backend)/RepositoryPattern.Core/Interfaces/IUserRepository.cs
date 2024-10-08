﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using RepositoryPattern.Core.DTOs;
using RepositoryPattern.Core.Models;
using RepositoryPattern.Core.ReturnedModels;
using RepositoryPatternUOW.Core.DTOs.Paymob.PaymobCardDto;
using RepositoryPatternWithUOW.Core.DTOs;
using RepositoryPatternWithUOW.Core.Enums;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.ReturnedModels;
using System.Linq.Expressions;


namespace RepositoryPattern.Core.Interfaces
{
    public interface IUserRepository 
    {
        public Task <LoginResult>LoginAsync(LoginDto loginDto);
        public Task<LoginResult> GoogleOAuthAsync(string accessToken);

        public Task <SignUpResult>SignUpAsync(SignUpDto signUpDto);
        public Task <bool> SendEmailVerificationAsync( string email, bool? IsForRestPassword = false);
        public Task<bool> ValidateConfirmationCodeAsync(string code, string email, bool? IsForRestPassword=false);
        public Task<TokensResult> UpdateTokensAsync(UpdateTokensDto updateTokensDto);
        public Task<bool> SignOutAsync(string refreshToken,string email);
        public Task<bool> DeleteRefreshTokenAsync(string refreshToken);
        public Task<SignUpResult> MakePatientAccount(MakePatientAccountDto makePatientAccountDto);

        public Task<SignUpResult> MakeDoctorAccount(MakeDoctorProfileDto createDoctorProfileDto);
        public Task<bool> ChangePassword(ChangePasswordDto changePasswordDto, int id);
        public Task<bool> UpdateDoctorProfilePicture(IFormFile? newImage, int doctorId);
        public Task<UpdateUserDataResult> UpdatePatient(JsonPatchDocument<Patient> document, int patientId);
        public Task<UpdateUserDataResult> UpdateDoctor(JsonPatchDocument<Doctor> document, int doctorId);

        public Task<PatientResult?> GetPatient(int id);
        
        public Task<ModifyInsensitveDataResult> ModifyInSensitiveDataAsync(JsonPatchDocument<User> modifyInsensitiveData,string password, int id);
        public Task<bool> DeleteAccountAsync<T>(int Id)where T:User;
        public Task<bool> VerifyPassword(string email, string password);

        public Task<IEnumerable<UsersResult>?> GetUsers<T>(int page)where T :User;
        public Task<UserProfileSettings?> GetUser(int id);
       
        public IEnumerable<UsersResult> SearchForUsers<T>(Expression<Func<T, bool>> expression,int page) where T : User;
        public Task<Patient?> GetPatientBy(Expression<Func<Patient, bool>> expression);//for searching
        public Task<UpdateUserDataResult> UpdateUserData<T>(UpdateUserDto user) where T:User;
        public Task<DoctorResult?> GetDoctor(int id);
        public IEnumerable<ScheduleResult> GetSchedulesOfDoctor(int id);
        public Task<ScheduleResult?> GetSchedule(int shiftId);
        public Task<ScheduleManipResult> AddSchedule(int doctorId, ScheduleDto scheduleDto);
        public Task<bool> DeleteSchedule(int shiftId);
        public IEnumerable<EmployeeCardResult> GetDoctorsOfDepartment(Department department, int page, int pageSize = 15);
        public IEnumerable<BookedAppointment> GetBookedAppointments(int doctorId,int page,int pageSize=20);
        public  Task<bool> PayAndBook(PaymobCardDto serviceDto);


        public Task<ScheduleManipResult> UpdateShift(int shiftId, JsonPatchDocument<Schedule> document);

        public Task<DoctorDetails?> GetDoctorDetails(int id);


    }
}
