namespace RepositoryPatternWithUOW.Core
{
    public static class CookiesGlobal
    {
        public static DateTime ExpirationOfJwt => DateTime.Now.AddMinutes(15);
        public static DateTime ExpirationOfRefreshToken => DateTime.Now.AddDays(1);
        public static string JwtCookieKey => "jwt";
        public static string RefreshTokenCookieKey => "refreshToken";
        public static string UserNameCookieKey => "userName";
        public static string EmailCookieKey => "email";
        public static string FirstNameCookieKey => "firstName";
        public static string LastNameCookieKey => "lastName";
        public static string GenderCookieKey  => "gender";
        public static string BirthDateCookieKey  => "birthDate";
        public static string RoleCookieKey => "role";
        public static string IdentityTokenVerificationKey => "IdentityToken";



        

    }
}
