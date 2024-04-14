using RepositoryPattern.Core.Interfaces;
using RepositoryPattern.EfCore;
using RepositoryPattern.EfCore.MailService;
using RepositoryPattern.EfCore.MapToModel;
using RepositoryPattern.EfCore.OptionPattenModels;
using RepositoryPattern.EfCore.Repositories;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.EfCore.MapToModel;
using RepositoryPatternWithUOW.EfCore.Repositories;
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
        public IDepartmentRepository DepartmentRepository { get;}
        public UnitOfWork(AppDbContext context,MapToUser mapToUser,MapToDepartment mapToDept,TokenOptionsModel tokenOptionsModel,IMailService mailService)
        {
            this.context=context;
            UserRepository = new UserRepositroy(context,mapToUser,tokenOptionsModel,mailService);
            DepartmentRepository = new DepartmentRepository(context,mapToDept);
        }
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
