
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

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            var result = await unitOfWork.UserRepository.SignUpAsync(signUpDto);
            if (!result.Success)
                return BadRequest(result);
            await unitOfWork.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("LogIn")]
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
       







        [HttpPost("SendCode")]
        public async Task<IActionResult> SendCode([FromBody] SendCodeDto sendCodeDto)
        {
            var result = await unitOfWork.UserRepository.SendEmailVerificationAsync(sendCodeDto.Email, sendCodeDto.Reset);
            if (!result)
                return NotFound();
            await unitOfWork.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("ValidateCode")]
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
        [HttpPatch("ModifyInSensitiveData")]
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
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var result = await unitOfWork.UserRepository.ChangePassword(changePasswordDto, ExtractJwt().Payload[JwtRegisteredClaimNames.NameId].ToString()!);
            if (!result)
                return BadRequest();
            await unitOfWork.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("UpdateTokens")]
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
        [HttpDelete("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            string? refToken = Request.Cookies[RefreshTokenCookieKey];
            if (refToken.IsNullOrEmpty())
                return BadRequest();
            var result = await unitOfWork.UserRepository.SignOutAsync(refToken);
            if (!result)
                return BadRequest();
            CookiesHandler.DeleteCookiesFromResponse(Response);


            return Ok();
        }
        [Authorize(Roles = "Adm")]
        [HttpPost("MakeDoctorAccount")]
        public async Task<IActionResult> MakeDoctorAccount(MakeDoctorProfileDto makeDoctorProfileDto)
        {
            var result = await unitOfWork.UserRepository.MakeDoctorAccount(makeDoctorProfileDto);
            if (!result)
                return BadRequest();
            await unitOfWork.SaveChangesAsync();
            return Ok();
        }
        [Authorize(Roles = "Adm")]
        [HttpDelete("DeleteAccount/{UserName}")]
        public async Task<IActionResult> DeleteAccount(string UserName)
        {
            var result = await unitOfWork.UserRepository.DeleteAccountAsync(UserName);
            if (result)
                return BadRequest();
            await unitOfWork.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost("VerifyPassword")]
        public async Task<IActionResult>VerifyPassword([FromBody]string password)
        {
            string email = ExtractJwt().Payload["email"].ToString()!;
            var result=await unitOfWork.UserRepository.VerifyPassword(email, password);
            return result ? Ok() : NotFound();
        }

    }
}
