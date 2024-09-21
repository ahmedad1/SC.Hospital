
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using RepositoryPatternUOW.Core.DTOs;
using RepositoryPatternUOW.Core.DTOs.Paymob.PaymobCardDto;
using RepositoryPatternWithUOW.Core.DTOs;
using RepositoryPatternWithUOW.Core.Enums;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.ReturnedModels;
using RepositoryPatternWithUOW.EfCore;
using RepositoryPatternWithUOW.EfCore.MapToModel;
using System.Collections.Frozen;
using System.Linq.Expressions;
using static RepositoryPatternWithUOW.Core.CookiesGlobal;
namespace RepositoryPattern.EfCore.Repositories
{
    public class UserRepositroy(AppDbContext context, MapToUser mapToUser,ScheduleMapper mapToSchedule, TokenOptionsModel JwtOptions, IMailService mailService,IWebHostEnvironment webHostEnvironment) : IUserRepository
    {
        AppDbContext context = context;
        MapToUser mapToUser = mapToUser;
        TokenOptionsModel JwtOptions = JwtOptions;
        IMailService mailService = mailService;

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
            var user =await context.Users.AsNoTracking().Include(x=>x.IdentityTokenVerification).Include(x=>x.VerificationCode).FirstOrDefaultAsync(x => x.Email == email);
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
            user.VerificationCode = new()
            {
                Code = verificationNum,
                ExpiresAt = DateTime.Now.AddSeconds(90),

            };
            context.Update(user);
            string token;
            DateTime expOfIdentityToken;
            if(user.IdentityTokenVerification is null)
            {
                token = Tokens.Generate();
                expOfIdentityToken = DateTime.Now.AddMinutes(25);
                user.IdentityTokenVerification = new() { ExpiresAt = expOfIdentityToken, Token = token };
            }
            else
            {
                expOfIdentityToken= user.IdentityTokenVerification.ExpiresAt;
                token = user.IdentityTokenVerification.Token;
                
            }

            Task t1= mailService.Send(email, subject,bodyOfMessage);
            Task t2 = context.SaveChangesAsync();
            await Task.WhenAll(t1, t2);

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
                return true;//make identity token

         
            user.EmailConfirmed = true;
          
            context.Update(user);
            return true;

            

            
        }
        async Task<string> SaveFile(IFormFile file)
        {
            var fileName = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
            var path = Path.Combine(webHostEnvironment.WebRootPath , fileName);
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            return fileName;
          

        }

        public async Task<SignUpResult> MakeDoctorAccount(MakeDoctorProfileDto makeDoctorProfileDto)
        {
            if(await context.Users.AnyAsync(x=>x.UserName== makeDoctorProfileDto.UserName ))
                return new() { Success=false,AlreadyExistField=nameof(AlreadyExistField.UserName)};
            if (await context.Users.AnyAsync(x => x.Email == makeDoctorProfileDto.Email ))
                return new() { Success=false, AlreadyExistField=nameof(AlreadyExistField.Email)};

                Doctor doctor = mapToUser.MapToDoctor(makeDoctorProfileDto);
                if(makeDoctorProfileDto.ProfilePicture is not null)
                doctor.ProfilePicture =await SaveFile(makeDoctorProfileDto.ProfilePicture);
                doctor.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(doctor.Password);
                await context.Doctors.AddAsync(doctor);
                return new() { Success=true};
        }
       public async Task<SignUpResult>MakePatientAccount(MakePatientAccountDto makePatientAccountDto)
        {
            if (await context.Users.AnyAsync(x => x.UserName == makePatientAccountDto.UserName))
                return new() { Success = false, AlreadyExistField = nameof(AlreadyExistField.UserName) };
            if (await context.Users.AnyAsync(x => x.Email == makePatientAccountDto.Email))
                return new() { Success = false, AlreadyExistField = nameof(AlreadyExistField.Email) };
            Patient patient = mapToUser.MapToPatient(makePatientAccountDto);
    
            patient.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(patient.Password);
            await context.Patients.AddAsync(patient);
            return new() { Success = true };
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
            else if(!refToken.IsActive)
            {
                context.Remove(refToken);
                return new() { Success = false };    
            }
            
         
            var newJwt = Tokens.Generate(refToken.User, JwtOptions,ExpirationOfJwt);
            var newRefToken = Tokens.Generate();
            await context.Set<RefreshToken>().Where(x => x.Token == refToken.Token).ExecuteDeleteAsync();

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
        public async Task<PatientResult?> GetPatient(int id)
        {

            return await context.Set<Patient>().Where(x => x.Id == id).Select(x => new PatientResult(id, x.FirstName, x.LastName, x.UserName, x.Email, x.EmailConfirmed, x.BirthDate.ToString(), x.Gender.ToString())).FirstOrDefaultAsync();
        }

        public async Task<UpdateUserDataResult> UpdateDoctor(JsonPatchDocument<Doctor>document,int doctorId)
        {
            List<string> allowed = ["firstName","lastname","username","email","password","gender","birthdate","department","EmailConfirmed","Biography","price"];
            if (!document.Operations.Exists(x => allowed.Exists(e => e.Equals(x.path, StringComparison.OrdinalIgnoreCase))))
               return new() { Success = false };
            
            try {
                var doctor =await context.Doctors.FindAsync(doctorId);
                if (doctor is null) return new() { Success = false };
                document.ApplyTo(doctor);
                if (document.Operations.Exists(x => x.path.Equals("password", StringComparison.OrdinalIgnoreCase)))
                    doctor.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(doctor.Password);
                if(document.Operations.Exists(x => x.path.Equals("userName", StringComparison.OrdinalIgnoreCase)))
                {
                    if(await context.Users.AnyAsync(x=>x.UserName==doctor.UserName))
                        return new (){Success=false,NewUserNameIsExist=true}; ;
                }
                if(document.Operations.Exists(x => x.path.Equals("email", StringComparison.OrdinalIgnoreCase)))
                {

                    if (await context.Users.AnyAsync(x => x.Email == doctor.Email))
                        return new() { Success = false, NewEmailIsExist = true }; 
                }
                return new() { NewEmailIsExist = false ,Success=true,NewUserNameIsExist=false};
            
            }
            catch {
                return new() { Success=false};
            }
        }
        public async Task<UpdateUserDataResult> UpdatePatient(JsonPatchDocument<Patient> document, int patientId)
        {
            List<string> allowed = ["firstName", "lastname", "username", "email", "password", "gender", "birthdate", "EmailConfirmed"];
            if (!document.Operations.Exists(x => allowed.Exists(e => e.Equals(x.path, StringComparison.OrdinalIgnoreCase))))
                return new() { Success = false };

            try
            {
                var patient = await context.Patients.FindAsync(patientId);
                if (patient is null) return new() { Success = false };
                document.ApplyTo(patient);
                if (document.Operations.Exists(x => x.path.Equals("password", StringComparison.OrdinalIgnoreCase)))
                    patient.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(patient.Password);
                if (document.Operations.Exists(x => x.path.Equals("userName", StringComparison.OrdinalIgnoreCase)))
                {
                    if (await context.Users.AnyAsync(x => x.UserName == patient.UserName))
                        return new() { Success = false, NewUserNameIsExist = true }; ;
                }
                if (document.Operations.Exists(x => x.path.Equals("email", StringComparison.OrdinalIgnoreCase)))
                {

                    if (await context.Users.AnyAsync(x => x.Email == patient.Email))
                        return new() { Success = false, NewEmailIsExist = true };
                }
                return new() { NewEmailIsExist = false, Success = true, NewUserNameIsExist = false };

            }
            catch
            {
                return new() { Success = false };
            }
        }
        public async Task<bool>UpdateDoctorProfilePicture(IFormFile? newImage,int doctorId)
        {
            var doctor = await context.Doctors.FindAsync(doctorId);
            if (doctor is null)
                return false;
            if(doctor.ProfilePicture is not null)
            {
                string path = Path.Combine(webHostEnvironment.WebRootPath, doctor.ProfilePicture);
                if (File.Exists(path))
                    File.Delete(path);
            }
            if (newImage is not null)
            {
                var newFile =await SaveFile(newImage);
                doctor.ProfilePicture = newFile;

            }
            return true;

        }
        public async Task<ModifyInsensitveDataResult> ModifyInSensitiveDataAsync(JsonPatchDocument<User> modifyInsensitiveData,string password,int id)
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
                var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                if (!BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password))
                {
                    return new() { Success = false, WrongPassword = true };
                }
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
        public async Task<bool> DeleteAccountAsync<T>(int Id)where T:User
        {
            
            var result=await context.Set<T>().Where(x => x.Id == Id).ExecuteDeleteAsync();
            if (result>0)
            return true;
            return false;

                
           

        }
    
       public async Task<DoctorResult?>GetDoctor(int id)
        {
            return await context.Set<Doctor>().Where(x => x.Id == id).Select(x => new DoctorResult(id, x.FirstName, x.LastName, x.UserName, x.Email, x.EmailConfirmed, x.BirthDate.ToString(), x.Gender.ToString(), x.Department.ToString(),x.Biography,x.Price)).FirstOrDefaultAsync();
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
        public async Task<UserProfileSettings?> GetUser(int id)
        {
            return await context.Users.Where(x => x.Id == id).Select(x => new UserProfileSettings(x.FirstName, x.LastName, x.UserName, x.Email, x is Patient ? "Pat" : x is Doctor ? "Doc" : "Adm", x.BirthDate.ToString(), x.Gender.ToString())).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UsersResult>?> GetUsers<T>(int page)where T:User
        {

            if (page <= 0)
                return Enumerable.Empty<UsersResult>();
           
            context.ChangeTracker.LazyLoadingEnabled = false;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            int pageSize = 20;
            IEnumerable<UsersResult> result;
            if (typeof(T) == typeof(Patient))
                result = await context.Set<Patient>().Skip(pageSize * (page - 1)).Take(pageSize).Select(x => new UsersResult(x.Id, x.FirstName, x.LastName, x.UserName, x.Email, x.Gender.ToString(), x.BirthDate, x.EmailConfirmed,null,null,null)).ToListAsync();
            else
            {
                result= await context.Set<Doctor>().Skip(pageSize * (page - 1)).Take(pageSize).Select(x => new UsersResult(x.Id, x.FirstName, x.LastName, x.UserName, x.Email, x.Gender.ToString(), x.BirthDate, x.EmailConfirmed,x.Department.ToString(),x.Price,x.Biography)).ToListAsync();
            }
            return result;
        }
        public IEnumerable<UsersResult> SearchForUsers<T>(Expression<Func<T, bool>> expression,int page) where T :User
        {
            if(page<=0)
                return Enumerable.Empty<UsersResult>();
     
            context.ChangeTracker.LazyLoadingEnabled = false;
            int pageSize=20;
            var result = context.Set<T>().Where(expression).Skip(pageSize * (page - 1)).Take(pageSize);
            if (typeof (T) == typeof (Patient))
            return result.Select(x => new UsersResult(x.Id, x.FirstName, x.LastName, x.UserName, x.Email, x.Gender.ToString(), x.BirthDate, x.EmailConfirmed,null,null,null)).AsNoTracking();
            else
            return result.Select(x => new UsersResult(x.Id, x.FirstName, x.LastName, x.UserName, x.Email, x.Gender.ToString(), x.BirthDate, x.EmailConfirmed, (x as Doctor)!.Department.ToString(),(x as Doctor)!.Price,(x as Doctor)!.Biography)).AsNoTracking();
            

        }
        public async Task<Patient?> GetPatientBy(Expression<Func<Patient, bool>> expression)
        {
            return await context.Patients.FirstOrDefaultAsync(expression);
        }
        public async Task<UpdateUserDataResult> UpdateUserData<T>(UpdateUserDto user)where T:User
        {
           try{
                if(await context.Users.AnyAsync(x=>x.Id!=user.Id&&(x.UserName==user.UserName)))
                {
                    return new() { NewUserNameIsExist = true ,Success=false,NewEmailIsExist=false};

                }
                if(await context.Users.AnyAsync(x => x.Id != user.Id && (x.Email == user.Email)))
                {
                    return new() { NewUserNameIsExist = false, Success = false, NewEmailIsExist = true };

                }
                var result = await context.Set<T>().Where(x => x.Id == user.Id).ExecuteUpdateAsync(u =>u
                .SetProperty(p => p.EmailConfirmed, user.EmailConfirmed)
                .SetProperty(p => p.BirthDate, user.BirthDate)
                .SetProperty(p => p.Email, user.Email)
                .SetProperty(p => p.FirstName, user.FirstName)
                .SetProperty(p => p.Gender, user.Gender)
                .SetProperty(p => p.LastName, user.LastName)
                .SetProperty(p=>p.UserName,user.UserName));
                return new() { Success=result!=0 , NewEmailIsExist=false,NewUserNameIsExist=false};
            }
            catch(Exception e){
                await Console.Out.WriteLineAsync(e.Message);
                return new() { Success =false, NewEmailIsExist = false, NewUserNameIsExist = false }; ;
            }

        }
        public IEnumerable<ScheduleResult> GetSchedulesOfDoctor(int id)
        {
            context.ChangeTracker.LazyLoadingEnabled = false;
            return  context.Schedules.Where(x => x.DoctorId == id).AsNoTracking().Select(x => new ScheduleResult(x.Id,x.Day, x.StartTime, x.EndTime));
        }
        public async Task<ScheduleResult?> GetSchedule(int shiftId)
        {
            context.ChangeTracker.LazyLoadingEnabled = false;
            return await context.Schedules.Where(x => x.Id==shiftId).AsNoTracking().Select(x => new ScheduleResult(x.Id,x.Day, x.StartTime, x.EndTime)).FirstOrDefaultAsync();
        }
        public async Task<ScheduleManipResult> AddSchedule(int doctorId , ScheduleDto scheduleDto)
        {
            if (await context.Schedules.AnyAsync(x => x.DoctorId == doctorId && x.Day == scheduleDto.Day))
                return new(false, true);
            var schedule = mapToSchedule.MapToSchedule(scheduleDto);
            schedule.DoctorId = doctorId;
            await context.Schedules.AddAsync(schedule);
            return new(true);
        }
        public async Task<bool> DeleteSchedule(int shiftId)
        {
            return await context.Schedules.Where(x => x.Id==shiftId).ExecuteDeleteAsync()>0;
        }
        public async Task<ScheduleManipResult> UpdateShift(int shiftId, JsonPatchDocument<Schedule> document)
        {
            List<string> allowed = ["StartTime","EndTime","Day"];
            if (document.Operations.Exists(x => !allowed.Exists(e => e.Equals(x.path, StringComparison.OrdinalIgnoreCase))))
            {
                return new(false);
            }
            try
            {
                var shift=await context.Schedules.FirstOrDefaultAsync(x=>x.Id==shiftId);
               
                if (shift is null)
                    return new (false);
                document.ApplyTo(shift);
                if (await context.Schedules.AnyAsync(x => x.Id!=shift.Id && x.Day == shift.Day&&x.DoctorId==shift.DoctorId))
                    return new(false, true);
                return new(true);

            }
            catch
            {
                return new(false);

            }
        }
        public  IEnumerable<EmployeeCardResult> GetDoctorsOfDepartment(Department department,int page,int pageSize=15)
        {
            return  context.Doctors.Where(x => x.Department == department).Select(x => new EmployeeCardResult(x.Id, x.FirstName, x.LastName, x.Biography, x.Price, x.ProfilePicture,x.Gender.ToString())).Skip((page-1)*pageSize).Take(pageSize).AsNoTracking();
        }
        public async Task<DoctorDetails?>GetDoctorDetails(int id)
        {
            context.ChangeTracker.LazyLoadingEnabled = false;
            return await context.Doctors.Where(x => x.Id == id).Select(x => new DoctorDetails(x.FirstName, x.LastName, x.Price, x.Biography, x.ProfilePicture,x.Schedules.Select(e=>new ScheduleResult(e.Id,e.Day,e.StartTime,e.EndTime)),x.Gender.ToString())).AsNoTracking().FirstOrDefaultAsync();
        }
        public IEnumerable<BookedAppointment> GetBookedAppointments(int doctorId,int page,int pageSize=20)
        {

            return context.Set<DoctorPatient>().Where(x => x.DoctorId == doctorId&&x.Appointment==DateOnly.FromDateTime(DateTime.Now)).OrderBy(x => x.Appointment).Skip((page - 1) * pageSize).Take(pageSize).Select(x => new BookedAppointment(x.Appointment, x.Patient.FirstName, x.Patient.LastName,x.Patient.Email)).AsNoTracking();
        }
        public async Task<bool> PayAndBook(PaymobCardDto serviceDto)
        {
            if (serviceDto.obj.order.items[0].description is null)
                return false;
            if (serviceDto.obj.success == false)
                return false;
            ServiceDescriptionPaymob? serviceDescriptionPaymob = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceDescriptionPaymob>(serviceDto.obj.order.items[0].description!);
            if (serviceDescriptionPaymob is null) return false;
            var email = serviceDto.obj.payment_key_claims.billing_data.email;
            if (string.IsNullOrEmpty(email))
                return false;
            if (!Enum.TryParse(serviceDescriptionPaymob.Date, true, out DateOnly date))
                return false;
            if (!await context.Schedules.AnyAsync(x => x.Day == date.DayOfWeek)) return false;

            if (await context.Set<DoctorPatient>().AnyAsync(x => x.PatientId == serviceDescriptionPaymob.PatientId && x.DoctorId == serviceDescriptionPaymob.DoctorId && x.Appointment.DayOfWeek==date.DayOfWeek))
                return false;
            await context.Set<DoctorPatient>().AddAsync(new DoctorPatient() { Appointment=date,DoctorId=serviceDescriptionPaymob.DoctorId,PatientId=serviceDescriptionPaymob.PatientId});
            return true;
        }
    }
}
