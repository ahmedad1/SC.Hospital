using Microsoft.IdentityModel.Tokens;
using RepositoryPattern.Core.Models;
using RepositoryPattern.EfCore.OptionPattenModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.EfCore.TokensHandler
{
    public class Tokens:IToken
    {

        public static string Generate(User user,TokenOptionsModel JwtOptions,DateTime expirationDate)
        {
            List<Claim> claimList = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId,user.UserName),
                new Claim(JwtRegisteredClaimNames.Name,user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName,user.LastName),
                new Claim(JwtRegisteredClaimNames.Birthdate,user.BirthDate.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Gender,user.Gender.ToString()),
                new Claim("id",user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.Discriminator)
            };
            var symmtericKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SigningKey));
            var signingCreds = new SigningCredentials(symmtericKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
               claims:claimList,
               issuer:JwtOptions.Issuer,
               audience:JwtOptions.Audience,
               expires:expirationDate,
               signingCredentials:signingCreds



                
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }
        public static string Generate()
        {
            byte[] rand = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(rand);
        }
    }
}
