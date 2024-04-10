
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


            return Ok(result);
        }
        [HttpPost("code-in-email")]
        public async Task<IActionResult> SendCode([FromBody] SendCodeDto sendCodeDto)
        {
            var result = await unitOfWork.UserRepository.SendEmailVerificationAsync(sendCodeDto.Email, sendCodeDto.Reset);
            if (!result)
                return NotFound();
            await unitOfWork.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("confirmation-code")]
        public async Task<IActionResult> ValidateCode(ValidateCodeDto validateCode)
        {
            var result = await unitOfWork.UserRepository.ValidateConfirmationCodeAsync(validateCode.Email, validateCode.Code, validateCode.Reset);
            await unitOfWork.SaveChangesAsync();
            if (!result)
                return BadRequest();
            return Ok();

        }
        private JwtSecurityToken ExtractJwt()
        {
            var jwt = Request.Cookies["jwt"];
            return new JwtSecurityTokenHandler().ReadJwtToken(jwt);

        }
        [Authorize]
        [HttpPatch("insensitive-data")]
        public async Task<IActionResult> ModifiyInsensitiveData(JsonPatchDocument<User> InsensitiveDto)
        {
            var email = ExtractJwt().Payload[JwtRegisteredClaimNames.Email].ToString();
            var result = await unitOfWork.UserRepository.ModifyInSensitiveDataAsync(InsensitiveDto, email!);
            if (!result.Success && result.HasRepeatedUserName)
                return BadRequest();
            else if (!result.Success && !result.HasRepeatedUserName)
                return NotFound();
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
            if (!result)
                return BadRequest();
            await unitOfWork.SaveChangesAsync();
            return Ok();
        }
        [Authorize(Roles = "Adm")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount([FromRoute(Name ="id")]int Id)
        {
            var result = await unitOfWork.UserRepository.DeleteAccountAsync(Id);
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
        [HttpPost("patients")]
        public async Task<IActionResult> GetPatients([FromBody]int page)
        {
            
            var result =await unitOfWork.UserRepository.GetPatients(page);
            return Ok(result);

        }
        [Authorize(Roles ="Adm")]
        [HttpPost("patients/{type-of-searching}")]
        public IActionResult SearchForPatients([FromRoute(Name="type-of-searching")]string typeOfSeaching,SearchDto searchDto)///next to do in front end -------------
        {
            if (!Enum.TryParse(typeOfSeaching, out TypeOfSearching result))
                return BadRequest();

            switch (result)
            {
                case TypeOfSearching.FirstName:
                    return Ok(unitOfWork.UserRepository.SearchForPatients(x => x.FirstName .Contains(searchDto.Data),searchDto.page));
                case TypeOfSearching.LastName:
                    return Ok(unitOfWork.UserRepository.SearchForPatients(x => x.LastName .Contains(searchDto.Data),searchDto.page));
                case TypeOfSearching.FullName:
                    return Ok(unitOfWork.UserRepository.SearchForPatients(x=>x.FirstName+" "+x.LastName==searchDto.Data,searchDto.page));
                case TypeOfSearching.Email:
                    return Ok(unitOfWork.UserRepository.SearchForPatients(x => x.Email.Contains(searchDto.Data),searchDto.page));
                case TypeOfSearching.UserName:
                    return Ok(unitOfWork.UserRepository.SearchForPatients(x => x.UserName.Contains( searchDto.Data),searchDto.page));
                case TypeOfSearching.EmailConfirmed:
                    return Ok(unitOfWork.UserRepository.SearchForPatients(x => x.EmailConfirmed.ToString() == searchDto.Data,searchDto.page));
                
                default: 
                    return BadRequest();

            }
            
        }


        [Authorize(Roles = "Adm")]
        [HttpPut("user")]
        public async Task<IActionResult> UpdateUserData(UpdateUserDto user)
        {
            var result = await unitOfWork.UserRepository.UpdateUserData(user);
            return result.Success ? Ok(result):BadRequest(result);
        }
    }
}
