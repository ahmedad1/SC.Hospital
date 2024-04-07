
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using RepositoryPattern.Core.DTOs;
using RepositoryPattern.Core.Interfaces;
using RepositoryPattern.Core.Models;
using RepositoryPattern.Core.ReturnedModels;
using RepositoryPattern.EfCore.MailService;
using RepositoryPattern.EfCore.MapToModel;
using RepositoryPattern.EfCore.OptionPattenModels;
using RepositoryPattern.EfCore.TokensHandler;
using RepositoryPatternWithUOW.Core.DTOs;
using RepositoryPatternWithUOW.Core.Enums;
using RepositoryPatternWithUOW.Core.ReturnedModels;
using System.Linq.Expressions;
using static RepositoryPatternWithUOW.Core.CookiesGlobal;
namespace RepositoryPattern.EfCore.Repositories
{
    public class UserRepositroy : IUserRepository
    {
        AppDbContext context;
        MapToUser mapToUser;
        TokenOptionsModel JwtOptions;
        IMailService mailService;
        public UserRepositroy(AppDbContext context, MapToUser mapToUser,TokenOptionsModel JwtOptions ,IMailService mailService)
        {
            this.context = context;
            this.mapToUser = mapToUser;
            this.JwtOptions = JwtOptions;
            this.mailService = mailService;
        }

        public async Task<LoginResult> LoginAsync(LoginDto obj)
        {
            context.ChangeTracker.LazyLoadingEnabled = false;

            var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == obj.UserName);
            if (user is null || !BCrypt.Net.BCrypt.EnhancedVerify(obj.Password, user.Password))
                return new()
                {
                    Success = false,
                    EmailConfirmed=false,
                };
            else if (user.EmailConfirmed==false)
                return new()
                {   
                    Email=user.Email,
                    Success = true,
                    EmailConfirmed = false,
                };
           
            var jwt=Tokens.Generate(user, JwtOptions,ExpirationOfJwt);
            context.Attach(user);
            context.ChangeTracker.LazyLoadingEnabled = true;

            if (user.RefreshToken.Count(x => x.IsActive) == 4)
                user.RefreshToken.Clear();
         
            string refToken = Tokens.Generate();
          
            user.RefreshToken.Add(new RefreshToken
            {
                CreatedAt = DateTime.Now,
                ExpiresAt = ExpirationOfRefreshToken,
                Token = refToken,

            });
          
            return new()
            {
               Success = true,
               EmailConfirmed=true,
               RefreshToken=refToken,
               Jwt=jwt
            };
        }
        public async Task<bool> SendEmailVerificationAsync(string email,bool? IsForRestPassword=false)
        {
            context.ChangeTracker.LazyLoadingEnabled=false;
            var user =await context.Users.AsNoTracking().Include(x=>x.VerificationCode).FirstOrDefaultAsync(x => x.Email == email);
            if (user is null)
                return false;
            if (user.VerificationCode is not null && user.VerificationCode.ExpiresAt < DateTime.Now.AddSeconds(7))
                context.Remove(user.VerificationCode);
            else if (user.VerificationCode is not null)
                return true;
            
            

            
            string bodyOfMessage;
            int verificationNum=new Random().Next(10000,9999999);
            string subject;
            if (IsForRestPassword is not null and true)
            {
                bodyOfMessage = @$"Dear {email} ,
                                    There was a request to reset your account on our SC.Hospital web application! 
                                    If you did not make this request then please ignore this email,
                                    and we have sent to you a verification code which is : <b>{verificationNum}</b> ";
                subject = "Reset Pasword";
            }
            else
            {
                bodyOfMessage = @$"Dear {email} ,
                                   you have signed up on our SC.Hospital web application, 
                                   and we have sent to you a verification code which is : <b>{verificationNum}</b> ";
                subject = "Email Confirmation";
            }
            await mailService.Send(email, subject,bodyOfMessage);
            user.VerificationCode = new()
            {
                Code = verificationNum,
                ExpiresAt = DateTime.Now.AddSeconds(90),

            };
            context.Update(user);
            return true;
        }
        public async Task<bool> ValidateConfirmationCodeAsync(string email , string code, bool? IsForRestPassword=false)
        {
            context.ChangeTracker.LazyLoadingEnabled = false;
            var user=await context.Users.Include(x=>x.VerificationCode).AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
            if (user is null ||user.VerificationCode is null)
                return false;
          
            
            
            if(user.VerificationCode.ExpiresAt<DateTime.Now)
            {   
                context.Remove(user.VerificationCode);
                return false;
            }
            context.Remove(user.VerificationCode);
            if (IsForRestPassword is not null and true)
                return true;
            
            
            user.EmailConfirmed = true;
          
            context.Update(user);
            return true;

            

            
        }
        public async Task<bool> MakeDoctorAccount(MakeDoctorProfileDto createDoctorProfileDto)
        {
            if (await context.Users.AnyAsync(x => x.Email == createDoctorProfileDto.Email || x.UserName == createDoctorProfileDto.UserName))
                return false;

                Doctor doctor = mapToUser.MapToDoctor(createDoctorProfileDto);
                doctor.EmailConfirmed = true;
                doctor.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(doctor.Password);
                await context.Doctors.AddAsync(doctor);
                return true;
        }
        public async Task<SignUpResult> SignUpAsync(SignUpDto signUpDto)
        {
            if (await context.Users.AnyAsync(x => x.Email == signUpDto.Email))
                return new() { Success=false,AlreadyExistField=nameof(AlreadyExistField.Email)};
            else if (await context.Users.AnyAsync(x=>x.UserName==signUpDto.UserName))
                return new() { Success = false, AlreadyExistField =nameof(AlreadyExistField.UserName) };

            try
            {
                Patient patient = mapToUser.MapToPatient(signUpDto);
                patient.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(patient.Password);
                await context.Patients.AddAsync(patient);
                return new (){Success=true};
            }
            catch
            {
                return new() { Success=false};
            }  
                
            
            
        }
        public async Task<TokensResult> UpdateTokensAsync(UpdateTokensDto updateTokensDto)
        {
           context.ChangeTracker.LazyLoadingEnabled = false;
           var refToken=await context.Set<RefreshToken>().Include(x=>x.User).AsNoTracking().FirstOrDefaultAsync(x => x.Token == updateTokensDto.RefreshToken);
            if (refToken is null )
                return new() { Success = false };
            else if(refToken.IsActive)
            {
                context.Remove(refToken);
                return new() { Success = false };    
            }
            
         
            var newJwt = Tokens.Generate(refToken.User, JwtOptions,ExpirationOfJwt);
            var newRefToken = Tokens.Generate();
            context.Remove(refToken);

            await context.AddAsync(new RefreshToken() { CreatedAt=DateTime.Now,ExpiresAt=ExpirationOfRefreshToken,Token=newRefToken,UserId=refToken.UserId});
            return new()
            {
                ExpirationOfJwt = ExpirationOfJwt,
                ExpirationOfRefreshToken = ExpirationOfRefreshToken,
                Jwt = newJwt,
                RefreshToken = newRefToken,
                Success=true
            };


        

        }
        public async Task<bool> DeleteRefreshTokenAsync(string refreshToken)
        {
           
           int result= await context.Set<RefreshToken>().Where(x => x.Token == refreshToken).ExecuteDeleteAsync();
            return result > 0;
            
            
          
        }
        public async Task<bool>ChangePassword(ChangePasswordDto changePasswordDto,int id)
        {
            context.ChangeTracker.LazyLoadingEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var user=await context.Users.FindAsync(id);
            if(user is null||!BCrypt.Net.BCrypt.EnhancedVerify(changePasswordDto.OldPassword,user.Password)) 
                return false;
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(changePasswordDto.NewPassword);
            context.Update(user);
            return true;
        }
        public async Task<ModifyInsensitveDataResult> ModifyInSensitiveDataAsync(JsonPatchDocument<User> modifyInsensitiveData,string email)
        {
            try
            {
                if (!modifyInsensitiveData.Operations.Exists(x =>
                   x.path.Equals("firstname", StringComparison.OrdinalIgnoreCase)
                || x.path.Equals("lastname", StringComparison.OrdinalIgnoreCase)
                || x.path.Equals("birthdate", StringComparison.OrdinalIgnoreCase)
                || x.path.Equals("gender", StringComparison.OrdinalIgnoreCase)
                || x.path.Equals("userName",StringComparison.OrdinalIgnoreCase))
                )
                {
                    return new() { Success = false };
                }
                context.ChangeTracker.LazyLoadingEnabled = false;
                var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
                if (user is null)
                    return new() { Success = false };
                modifyInsensitiveData.ApplyTo(user);
                context.Update(user);
                return new() { Success = true };


            }
            catch (DbUpdateException e)
            {

                return new() { Success=false,HasRepeatedUserName=true};
            }
            catch
            {
                return new() { Success = false };
            }
        }
        public async Task<bool> DeleteAccountAsync(string username)
        {
            
            var results=await context.Users.Where(x => x.UserName == username).ExecuteDeleteAsync();
            if (results>0)
            return true;
            return false;

                
           

        }
        public async Task<bool>AddSchedulesForDoctor(SchedulesDto schedulesDto)//try catch
        {
            context.ChangeTracker.LazyLoadingEnabled = false;
            var doctor =await context.Doctors.AsNoTracking().Include(x=>x.SchedualsOfDoctor).FirstOrDefaultAsync(x => x.UserName == schedulesDto.userName);
            if(doctor is null ) 
                return false;
            
                doctor.SchedualsOfDoctor.Clear();
                for (int i = 0; i < schedulesDto.Schedules.Count(); i++)
                {
                    doctor.SchedualsOfDoctor.Add(new() { Schedule = schedulesDto.Schedules[i] });
                }
            
           
            return true;
        }

        public async Task<bool> VerifyPassword(string email, string password)
        {
            context.ChangeTracker.LazyLoadingEnabled = false;
            var user=await context.Users.AsNoTracking().Select(x=>new {x.Email, x.Password}).FirstOrDefaultAsync(x => x.Email == email);
            return user is not null && BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password);
                

        }

        public async Task<bool> SignOutAsync(string refreshToken, string email)
        {
           var refToken=await context.Set<RefreshToken>().Include(x=>x.User).AsNoTracking().FirstOrDefaultAsync(x=>x.Token==refreshToken);
            if(refToken is null||refToken.User.Email!=email) 
                return false;

            context.Remove(refToken);
            return true;
        }
        public async Task<IEnumerable<UsersResult>?> GetPatients(int page)
        {

            if (page <= 0)
                return Enumerable.Empty<UsersResult>();
            context.ChangeTracker.LazyLoadingEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            int pageSize = 20;
            var result = await context.Patients.Skip(pageSize * (page - 1)).Take(pageSize).Select(x => new UsersResult(x.Id, x.FirstName, x.LastName, x.UserName, x.Email, x.Gender.ToString(), x.BirthDate, x.EmailConfirmed)).ToListAsync();
     
            return result;
        }
        public IEnumerable<UsersResult> SearchForPatients(Expression<Func<Patient, bool>> expression,int page)
        {
            if(page<=0)
                return Enumerable.Empty<UsersResult>();
            context.ChangeTracker.LazyLoadingEnabled = false;
            int pageSize=20;
            return context.Patients.Where(expression).Skip(pageSize*(page-1)).Take(pageSize).Select(x => new UsersResult(x.Id, x.FirstName, x.LastName, x.UserName, x.Email, x.Gender.ToString(), x.BirthDate, x.EmailConfirmed)).AsNoTracking();
        }
        public async Task<Patient?> GetPatientBy(Expression<Func<Patient, bool>> expression)
        {
            return await context.Patients.FirstOrDefaultAsync(expression);
        }
    }
}
