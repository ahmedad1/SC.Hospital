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

        public Task<bool> MakeDoctorAccount(MakeDoctorProfileDto createDoctorProfileDto);
        public Task<bool> ChangePassword(ChangePasswordDto changePasswordDto, int id);

        public Task<ModifyInsensitveDataResult> ModifyInSensitiveDataAsync(JsonPatchDocument<User> modifyInsensitiveData, string email);
        public Task<bool> DeleteAccountAsync(string username);
        public Task<bool> VerifyPassword(string email, string password);

        public Task<IEnumerable<UsersResult>?> GetPatients(int page);
        public IEnumerable<UsersResult> SearchForPatients(Expression<Func<Patient, bool>> expression,int page);
        public Task<Patient?> GetPatientBy(Expression<Func<Patient, bool>> expression);//for searching

    }
}
