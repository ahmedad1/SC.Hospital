using Microsoft.AspNetCore.JsonPatch;
using RepositoryPattern.Core.DTOs;
using RepositoryPattern.Core.Models;
using RepositoryPattern.Core.ReturnedModels;
using RepositoryPatternWithUOW.Core.DTOs;
using RepositoryPatternWithUOW.Core.ReturnedModels;
using System.Linq.Expressions;


namespace RepositoryPattern.Core.Interfaces
{
    public interface IUserRepository 
    {
        public Task <LoginResult>LoginAsync(LoginDto loginDto);
        public Task <SignUpResult>SignUpAsync(SignUpDto signUpDto);
        public Task <bool> SendEmailVerificationAsync( string email, bool? IsForRestPassword = false);
        public Task<bool> ValidateConfirmationCodeAsync(string code, string email, bool? IsForRestPassword=false);
        public Task<TokensResult> UpdateTokensAsync(UpdateTokensDto updateTokensDto);
        public Task<bool> SignOutAsync(string refreshToken,string email);
        public Task<bool> DeleteRefreshTokenAsync(string refreshToken);
        public Task<SignUpResult> MakePatientAccount(MakePatientAccountDto makePatientAccountDto);

        public Task<SignUpResult> MakeDoctorAccount(MakeDoctorProfileDto createDoctorProfileDto);
        public Task<bool> ChangePassword(ChangePasswordDto changePasswordDto, int id);

        public Task<ModifyInsensitveDataResult> ModifyInSensitiveDataAsync(JsonPatchDocument<User> modifyInsensitiveData,string password, int id);
        public Task<bool> DeleteAccountAsync<T>(int Id)where T:User;
        public Task<bool> VerifyPassword(string email, string password);

        public Task<IEnumerable<UsersResult>?> GetUsers<T>(int page)where T :User;
        public Task<UserProfileSettings?> GetUser(int id);
       
        public IEnumerable<UsersResult> SearchForUsers<T>(Expression<Func<T, bool>> expression,int page) where T : User;
        public Task<Patient?> GetPatientBy(Expression<Func<Patient, bool>> expression);//for searching
        public Task<UpdateUserDataResult> UpdateUserData<T>(UpdateUserDto user) where T:User;
        public Task<DoctorResult?> GetDoctor(int id);

    }
}
