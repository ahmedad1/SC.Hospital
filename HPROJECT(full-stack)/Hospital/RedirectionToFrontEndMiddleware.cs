﻿using Microsoft.IdentityModel.Tokens;
using RepositoryPatternWithUOW.Core.Interfaces;
using static RepositoryPatternWithUOW.Core.CookiesGlobal;

namespace Hospital
{
    public class RedirectionToFrontEndMiddleware( RequestDelegate next)
    {

        public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
        {
            
            if (context.Request.Path=="/")
            {
                
                if (context.Request.Cookies.TryGetValue(RefreshTokenCookieKey, out string? refToken) && context.Request.Cookies.TryGetValue(RoleCookieKey, out string? role))
                {
                    if (role == "Pat")
                        context.Response.Redirect("/user.html");
                    else if (role == "Doc")
                        context.Response.Redirect("/doctor.html");
                    else if (role == "Adm")
                        context.Response.Redirect("/admin.html");

                }
                else
                {
                    if (!refToken.IsNullOrEmpty())
                    {
                    
                        await unitOfWork.UserRepository.DeleteRefreshTokenAsync(refToken);
                        CookiesHandler.DeleteCookiesFromResponse(context.Response);
                    }
                
                    context.Response.Redirect("/index.html");
                


                }
                return;
            }
            await next(context);
        }

    }
}
