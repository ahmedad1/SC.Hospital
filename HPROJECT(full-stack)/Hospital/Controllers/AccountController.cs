
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
        [Authorize(Roles = "Adm")]
        [HttpPost("new-doctor-account")]
        public async Task<IActionResult> MakeDoctorAccount(MakeDoctorProfileDto makeDoctorProfileDto)
        {
            var result = await unitOfWork.UserRepository.MakeDoctorAccount(makeDoctorProfileDto);
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
        [HttpGet("patients/{type-of-searching}")]
        public IActionResult SearchForPatients([FromRoute(Name="type-of-searching")]string typeOfSeaching,SearchDto searchDto)///next to do in front end -------------
        {
            if (!Enum.TryParse(typeOfSeaching, out TypeOfSearching result))
                return BadRequest();
            
            switch (result)
            {
                case TypeOfSearching.FirstName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Patient>(x => x.FirstName .Contains(searchDto.Data),searchDto.page));
                case TypeOfSearching.LastName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Patient>(x => x.LastName .Contains(searchDto.Data),searchDto.page));
                case TypeOfSearching.FullName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Patient>(x=>x.FirstName+" "+x.LastName==searchDto.Data,searchDto.page));
                case TypeOfSearching.Email:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Patient>(x => x.Email.Contains(searchDto.Data),searchDto.page));
                case TypeOfSearching.UserName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Patient>(x => x.UserName.Contains( searchDto.Data),searchDto.page));
                case TypeOfSearching.EmailConfirmed:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Patient>(x => x.EmailConfirmed.ToString() == searchDto.Data,searchDto.page));
                
                default: 
                    return BadRequest();

            }
            
        }
        [Authorize(Roles ="Adm")]
        [HttpGet("doctors/{type-of-searching}")]
        public IActionResult SearchForDoctors([FromRoute(Name = "type-of-searching")] string typeOfSeaching, SearchDto searchDto)
        {
            if (!Enum.TryParse(typeOfSeaching, out TypeOfSearching result))
                return BadRequest();

            switch (result)
            {
                case TypeOfSearching.FirstName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Doctor>(x => x.FirstName.Contains(searchDto.Data), searchDto.page));
                case TypeOfSearching.LastName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Doctor>(x => x.LastName.Contains(searchDto.Data), searchDto.page));
                case TypeOfSearching.FullName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Doctor>(x => x.FirstName + " " + x.LastName == searchDto.Data, searchDto.page));
                case TypeOfSearching.Email:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Doctor>(x => x.Email.Contains(searchDto.Data), searchDto.page));
                case TypeOfSearching.UserName:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Doctor>(x => x.UserName.Contains(searchDto.Data), searchDto.page));
                case TypeOfSearching.EmailConfirmed:
                    return Ok(unitOfWork.UserRepository.SearchForUsers<Doctor>(x => x.EmailConfirmed.ToString() == searchDto.Data, searchDto.page));

                default:
                    return BadRequest();

            }

        }
        [Authorize(Roles ="Adm")]
        [HttpGet("{role}/{id}")]
        public async Task<IActionResult>GetUserForAdmin(string role,int id)
        {
            if (!Enum.TryParse(role, true, out Role val))
                return BadRequest();
            if (val == Role.Patients)
            {
                var result = await unitOfWork.UserRepository.GetUser(id);

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
        [HttpPut("user")]
        public async Task<IActionResult> UpdateUserData(UpdateUserDto user)
        {
            UpdateUserDataResult result;

            if(user.Role==Role.Patients)
            result = await unitOfWork.UserRepository.UpdateUserData<Patient>(user);
            else
            result = await unitOfWork.UserRepository.UpdateUserData<Doctor>(user);

            return result.Success ? Ok(result):BadRequest(result);
        }
    }
}
