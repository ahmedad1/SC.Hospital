using RepositoryPattern.Core.Interfaces;
using RepositoryPattern.EfCore;
using RepositoryPattern.EfCore.MailService;
using RepositoryPattern.EfCore.MapToModel;
using RepositoryPattern.EfCore.OptionPattenModels;
using RepositoryPattern.EfCore.Repositories;
using RepositoryPatternWithUOW.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EfCore
{
    public class UnitOfWork : IUnitOfWork
    {
        AppDbContext context;
        
        public IUserRepository UserRepository { get; }
        public UnitOfWork(AppDbContext context,MapToUser mapper,TokenOptionsModel tokenOptionsModel,IMailService mailService)
        {
            this.context=context;
            UserRepository = new UserRepositroy(context,mapper,tokenOptionsModel,mailService);

        }
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
