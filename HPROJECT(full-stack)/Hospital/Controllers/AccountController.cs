
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RepositoryPattern.Core.DTOs;
using RepositoryPattern.Core.Interfaces;
using RepositoryPattern.Core.Models;
using RepositoryPatternWithUOW.Core.DTOs;
using RepositoryPatternWithUOW.Core.Enums;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.ReturnedModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static RepositoryPatternWithUOW.Core.CookiesGlobal;
namespace Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IUnitOfWork unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            var result = await unitOfWork.UserRepository.SignUpAsync(signUpDto);
            if (!result.Success)
                return BadRequest(result);
            await unitOfWork.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("log-in")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await unitOfWork.UserRepository.LoginAsync(loginDto);
            if (!result.Success)
                return NotFound();

            if (!result.EmailConfirmed)
            {
                CookiesHandler.SetCookie("email",result.Email,DateTime.Now.AddDays(1),false,Response);    
                return Ok(new {EmailConfirmed=false,Success=true});
            }
            await unitOfWork.SaveChangesAsync();
            var jwtObj = new JwtSecurityTokenHandler().ReadJwtToken(result.Jwt);
            var payload = jwtObj.Payload;
            CookiesHandler.SetCookiesInResponse(payload, result, Response);


            return Ok();
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "id");
            if (id is null)
                return BadRequest();

            var result = await unitOfWork.UserRepository.GetUser(int.Parse(id.Value));
            return result is null ? BadRequest() : Ok(result);
        }
        [HttpPost("code-in-email")]
        public async Task<IActionResult> SendCode([FromBody] SendCodeDto sendCodeDto)
        {
            var result = await unitOfWork.UserRepository.SendEmailVerificationAsync(sendCodeDto.Email, sendCodeDto.Reset);
            if (!result)
                return NotFound();
            
            return Ok();
        }
        [HttpPost("confirmation-code")]
        public async Task<IActionResult> ValidateCode(ValidateCodeDto validateCode)
        {
          
            var result = await unitOfWork.UserRepository.ValidateConfirmationCodeAsync(validateCode.Email, validateCode.Code, validateCode.Reset);
            await unitOfWork.SaveChangesAsync();
            if (!result)
                return BadRequest();
            CookiesHandler.SetCookie(IdentityTokenVerificationKey,"",DateTime.Now.AddDays(-5),true,Response);
            return Ok();

        }
        private JwtSecurityToken ExtractJwt()
        {
            var jwt = Request.Cookies["jwt"];
            return new JwtSecurityTokenHandler().ReadJwtToken(jwt);

        }
        [Authorize]
        [HttpPatch("insensitive-data")]
        public async Task<IActionResult> ModifiyInsensitiveData(ModifyInsensitveDataPatch modifyInsensitveDataPatch)
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "id");
            if (id is null)
                return BadRequest();
            var result = await unitOfWork.UserRepository.ModifyInSensitiveDataAsync(modifyInsensitveDataPatch.User,modifyInsensitveDataPatch.Password, int.Parse(id.Value));
            if (!result.Success)
                return BadRequest(result);
            
            await unitOfWork.SaveChangesAsync();
            return Ok();
        }
        [Authorize]
        [HttpPost("password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            string? idString = ExtractJwt().Payload["id"].ToString()!;
            if (idString is null || !int.TryParse(idString, out int id))
                return BadRequest();
            var result = await unitOfWork.UserRepository.ChangePassword(changePasswordDto,id);
            if (!result)
                return BadRequest();
            await unitOfWork.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("tokens")]
        public async Task<IActionResult> UpdateTokens()
        {
            if (!Request.Cookies.TryGetValue(RefreshTokenCookieKey,out string? refToken)||!Request.Cookies.TryGetValue(UserNameCookieKey,out string? userName))
                return Unauthorized();
            
            
            var result = await unitOfWork.UserRepository.UpdateTokensAsync(new() { RefreshToken = refToken, UserName = userName });
            await unitOfWork.SaveChangesAsync();
            if (!result.Success)
                return Unauthorized();
            CookiesHandler.SetCookie(JwtCookieKey, result.Jwt, ExpirationOfJwt, true,Response);
            CookiesHandler.SetCookie(RefreshTokenCookieKey, result.RefreshToken, ExpirationOfRefreshToken, true, Response);
            return Ok();
        }
        [HttpDelete("sign-out")]
        public async Task<IActionResult> SignOut()
        {
            string? refToken = Request.Cookies[RefreshTokenCookieKey];
            if (refToken.IsNullOrEmpty())
                return BadRequest();
            string?userName= Request.Cookies[EmailCookieKey];
            if (userName.IsNullOrEmpty())
                return BadRequest();

            var result = await unitOfWork.UserRepository.SignOutAsync(refToken,userName);
            if (!result)
                return BadRequest();
            await unitOfWork.SaveChangesAsync();
            CookiesHandler.DeleteCookiesFromResponse(Response);


            return Ok();
        }
        bool IsImage(IFormFile? file)
        {
            if(file is null) return true;
            if (file.ContentType.StartsWith("image/") && Path.GetExtension(file.FileName) is ".jpg" or ".jpeg" or ".png" or ".webp")
                return true;
            return false;
        }
        [Authorize(Roles = "Adm")]
        [HttpPost("new-doctor-account")]
        public async Task<IActionResult> MakeDoctorAccount([FromForm]MakeDoctorProfileDto makeDoctorProfileDto)
        {
            if (!IsImage(makeDoctorProfileDto.ProfilePicture))
                return BadRequest();
            var result = await unitOfWork.UserRepository.MakeDoctorAccount(makeDoctorProfileDto);
            if (!result.Success)
                return BadRequest(result);
            await unitOfWork.SaveChangesAsync();
            return Ok(result);
        }
        [Authorize(Roles = "Adm")]
        [HttpPost("new-patient-account")]
        public async Task<IActionResult> MakePatientAccount([FromForm] MakePatientAccountDto makePatientAccountDto)
        {
            var result = await unitOfWork.UserRepository.MakePatientAccount(makePatientAccountDto);
            if (!result.Success)
                return BadRequest(result);
            await unitOfWork.SaveChangesAsync();
            return Ok(result);
        }
        [Authorize(Roles = "Adm")]
        [HttpDelete("{role}/{id}")]
        public async Task<IActionResult> DeleteAccount([FromRoute(Name ="id")]int id,[FromRoute]string role)
        {
            if (!Enum.TryParse(role,true, out Role val))
                return BadRequest();
            bool result;
            if(val==Role.Patients)
            result=await unitOfWork.UserRepository.DeleteAccountAsync<Patient>(id);
            else
            result=await unitOfWork.UserRepository.DeleteAccountAsync<Doctor>(id);
            if (!result)
                return BadRequest();
            return Ok();
        }
        
        [Authorize]
        [HttpPost("verifying-password")]
        public async Task<IActionResult>VerifyPassword([FromBody]string password)
        {
            string email = ExtractJwt().Payload["email"].ToString()!;
            var result=await unitOfWork.UserRepository.VerifyPassword(email, password);
            return result ? Ok() : NotFound();
        }
        [Authorize(Roles = "Adm")]
        [HttpGet("patients")]
        public async Task<IActionResult> GetPatients([FromQuery]int page)
        {
            
            var result =await unitOfWork.UserRepository.GetUsers<Patient>(page);
            return Ok(result);

        }
        [Authorize(Roles ="Adm")]
        [HttpGet("doctors")]
        public async Task<IActionResult>GetDoctors([FromQuery]int page)
        {
            var result = await unitOfWork.UserRepository.GetUsers<Doctor>(page);
            return Ok(result);

        }
        [Authorize(Roles ="Adm")]
        [HttpGet("patient")]
        public IActionResult SearchForPatients([FromQuery(Name = "searchKey")] string typeOfSeaching, [FromQuery(Name = "searchValue")] string data, [FromQuery] int page)///next to do in front end -------------
        {
            if (!Enum.TryParse(typeOfSeaching,true, out TypeOfSearching result))
                return BadRequest();
            
            switch (result)
            {
                case TypeOfSearching.FirstName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Patient>(x => x.FirstName .StartsWith(data),page));
                case TypeOfSearching.LastName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Patient>(x => x.LastName .StartsWith(data),page));
                case TypeOfSearching.FullName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Patient>(x=>(x.FirstName+" "+x.LastName).StartsWith(data),page));
                case TypeOfSearching.Email:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Patient>(x => x.Email.StartsWith(data),page));
                case TypeOfSearching.UserName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Patient>(x => x.UserName.StartsWith( data),page));
                case TypeOfSearching.EmailConfirmed:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Patient>(x => x.EmailConfirmed.ToString() == data,page));
                
                default: 
                    return BadRequest();

            }
            
        }
        [Authorize(Roles ="Adm")]
        [HttpGet("doctor")]
        public IActionResult SearchForDoctors([FromQuery(Name = "searchKey")] string typeOfSeaching, [FromQuery(Name ="searchValue")]string data, [FromQuery]int page)
        {
            if (!Enum.TryParse(typeOfSeaching,true, out TypeOfSearching result))
                return BadRequest();

            switch (result)
            {
                case TypeOfSearching.FirstName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Doctor>(x => x.FirstName.StartsWith(data), page));
                case TypeOfSearching.LastName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Doctor>(x => x.LastName.StartsWith(data), page));
                case TypeOfSearching.FullName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Doctor>(x =>( x.FirstName + " " + x.LastName).StartsWith(data), page));
                case TypeOfSearching.Email:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Doctor>(x => x.Email.StartsWith(data), page));
                case TypeOfSearching.UserName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Doctor>(x => x.UserName.StartsWith(data), page));
                case TypeOfSearching.EmailConfirmed:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Doctor>(x => x.EmailConfirmed.ToString() == data, page));
                case TypeOfSearching.Department:
                    if (!Enum.TryParse( data,true, out Department department))
                        return BadRequest();
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Doctor>(x=>x.Department==department,page));

                default:
                    return BadRequest();

            }

        }
        [Authorize(Roles ="Adm")]
        [HttpGet("{id}")]
        public async Task<IActionResult>GetUserForAdmin([FromQuery]string role,int id)
        {
            if (!Enum.TryParse(role, true, out Role val))
                return BadRequest();
            if (val == Role.Patients)
            {
                var result = await unitOfWork.UserRepository.GetPatient(id);

                if (result == null)
                    return BadRequest();
                return Ok(result);
            }
            else
            {
                var result = await unitOfWork.UserRepository.GetDoctor(id);
                if (result == null)
                    return BadRequest();
                return Ok(result);
            }
        }

        [Authorize(Roles = "Adm")]
        [HttpPatch("doctor/{id}")]
        public async Task<IActionResult> UpdateDoctorData(JsonPatchDocument<Doctor>doctorDocument,int id)
        {
            var result = await unitOfWork.UserRepository.UpdateDoctor(doctorDocument, id);
            if (result.Success)
                await unitOfWork.SaveChangesAsync();
            return result.Success ? Ok() : BadRequest(result) ;
        }
        [Authorize(Roles ="Adm")]
        [HttpPatch("patient/{id}")]
        public async Task<IActionResult> UpdatePatientData(JsonPatchDocument<Patient> patientDocument,int id)
        {
            var result = await unitOfWork.UserRepository.UpdatePatient(patientDocument, id);
            if (result.Success)
                await unitOfWork.SaveChangesAsync();
            return result.Success ? Ok() : BadRequest(result);
        }
        [Authorize(Roles = "Adm")]
        [HttpPut("{id}/profile-picture")]
        public async Task<IActionResult> UpdateProfilePictureOfDoctor([FromForm] IFormFile? image, int id)
        {
            var result = await unitOfWork.UserRepository.UpdateDoctorProfilePicture(image, id);
            if (result)
                await unitOfWork.SaveChangesAsync();
            return Ok();
        }
        [Authorize(Roles ="Adm")]
        [HttpGet("{id}/schedules")]
        public IActionResult GetSchedulesOfEmployee(int id)
        {
            return Ok( unitOfWork.UserRepository.GetSchedulesOfDoctor(id));
        }
        [Authorize(Roles ="Pat")]
        [HttpGet("doctor/{id}")]
        public async Task<IActionResult>GetDoctorDetails(int id)
        {
            var result = await unitOfWork.UserRepository.GetDoctorDetails(id);

            return result is not null? Ok(result):BadRequest();
        }
        [Authorize(Roles ="Adm")]
        [HttpGet("doctor/schedule/{shiftId}")]
        public async Task<IActionResult> GetScheduleOfEmployee(int shiftId)
        {
            var result = await unitOfWork.UserRepository.GetSchedule(shiftId);
            return result is not null?Ok(result ):BadRequest();
        }
        [Authorize(Roles ="Adm")]
        [HttpPost("{id}/schedule")]
        public async Task<IActionResult> AddScheduleForDoctor(int id,ScheduleDto scheduleDto)
        {
            var result = await unitOfWork.UserRepository.AddSchedule(id, scheduleDto);
            if (result)
            {
                try
                {
                    await unitOfWork.SaveChangesAsync();
                }
                catch
                {
                    return BadRequest();
                }
            }
            return result ?Ok():BadRequest();
        }
        [Authorize(Roles ="Adm")]
        [HttpDelete("doctor/schedule/{shiftId}")]
        public async Task<IActionResult> DeleteSchedule(int shiftId)
        {
            var result = await unitOfWork.UserRepository.DeleteSchedule(shiftId);
            return result ?Ok():BadRequest();
        }
        [Authorize(Roles ="Adm")]
        [HttpPatch("doctor/schedule/{shiftId}")]
        public async Task<IActionResult> UpdateShift(int shiftId, [FromBody] JsonPatchDocument<Schedule> document)
        {
            var result=await unitOfWork.UserRepository.UpdateShift(shiftId,document);

            if(result)
            {
                try
                {
                  
                    await unitOfWork.SaveChangesAsync();
                    return Ok();
                }
                catch
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }
        [Authorize (Roles ="Pat")]
        [HttpGet("department/doctors")]
        public  IActionResult GetDoctorsOfDepartment(Department department,int page)
        {
            return Ok(unitOfWork.UserRepository.GetDoctorsOfDepartment(department,page));
        }


    }
}
