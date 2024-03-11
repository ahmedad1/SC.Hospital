using Azure;
using RepositoryPattern.Core.ReturnedModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static RepositoryPatternWithUOW.Core.CookiesGlobal;

namespace Hospital
{
    public class CookiesHandler
    {
        public static void SetCookie(string key, string value, DateTime exp, bool httpOnly,HttpResponse response)
        {
            var cookieOpts = new CookieOptions();
            cookieOpts.Expires = exp;
            cookieOpts.Secure = true;
            cookieOpts.HttpOnly = httpOnly;
            cookieOpts.SameSite = SameSiteMode.Strict;
            response.Cookies.Append(key, value, cookieOpts);



        }

        public static void SetCookiesInResponse(JwtPayload payload,LoginResult result,HttpResponse response)
        {
            SetCookie(JwtCookieKey, result.Jwt!, ExpirationOfJwt, true,response);
            SetCookie(RefreshTokenCookieKey, result.RefreshToken!, ExpirationOfRefreshToken, true,response);
            SetCookie(FirstNameCookieKey, payload[JwtRegisteredClaimNames.Name].ToString()!, ExpirationOfRefreshToken, false,response);
            SetCookie(LastNameCookieKey, payload[JwtRegisteredClaimNames.FamilyName].ToString()!, ExpirationOfRefreshToken, false,response);
            SetCookie(UserNameCookieKey, payload[JwtRegisteredClaimNames.NameId].ToString()!, ExpirationOfRefreshToken, false,response);
            SetCookie(GenderCookieKey, payload[JwtRegisteredClaimNames.Gender].ToString()!, ExpirationOfRefreshToken, false,response);
            SetCookie(BirthDateCookieKey, payload[JwtRegisteredClaimNames.Birthdate].ToString()!, ExpirationOfRefreshToken, false,response);
            SetCookie(RoleCookieKey, payload[ClaimTypes.Role].ToString()!, ExpirationOfRefreshToken, false,response);
            SetCookie(EmailCookieKey, payload[JwtRegisteredClaimNames.Email].ToString()!, ExpirationOfRefreshToken, false,response);

        }
        public static void DeleteCookiesFromResponse( HttpResponse response)
        {
            SetCookie(JwtCookieKey, "", DateTime.Now.AddSeconds(-9), true,response);
            SetCookie(RefreshTokenCookieKey, "", DateTime.Now.AddSeconds(-9), true,response);
            SetCookie(FirstNameCookieKey, "", DateTime.Now.AddSeconds(-9), false,response);
            SetCookie(LastNameCookieKey, "", DateTime.Now.AddSeconds(-9), false,response);
            SetCookie(UserNameCookieKey, "", DateTime.Now.AddSeconds(-9), false,response);
            SetCookie(GenderCookieKey, "", DateTime.Now.AddSeconds(-9), false,response);
            SetCookie(BirthDateCookieKey, "", DateTime.Now.AddSeconds(-9), false,response);
            SetCookie(RoleCookieKey, "", DateTime.Now.AddSeconds(-9), false,response);
            SetCookie(EmailCookieKey, "", DateTime.Now.AddSeconds(-9), false,response);
        }
    }
}
