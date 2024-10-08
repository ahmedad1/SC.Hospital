using RepositoryPattern.Core.Models;
using RepositoryPattern.EfCore.OptionPattenModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.EfCore.TokensHandler
{
    public  interface IToken
    {

        public static abstract string Generate(User user, TokenOptionsModel JwtOptions, DateTime expirationDate);
        public static abstract string Generate();

    }
}
