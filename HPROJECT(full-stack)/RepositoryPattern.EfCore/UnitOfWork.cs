using Microsoft.AspNetCore.Hosting;
using RepositoryPattern.Core.Interfaces;
using RepositoryPattern.EfCore;
using RepositoryPattern.EfCore.MailService;
using RepositoryPattern.EfCore.MapToModel;
using RepositoryPattern.EfCore.OptionPattenModels;
using RepositoryPattern.EfCore.Repositories;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.EfCore.MapToModel;


namespace RepositoryPatternWithUOW.EfCore
{
    public class UnitOfWork : IUnitOfWork
    {
        AppDbContext context;

        public IUserRepository UserRepository { get; }
        public UnitOfWork(AppDbContext context,ScheduleMapper scheduleMapper,MapToUser mapToUser,TokenOptionsModel tokenOptionsModel,IMailService mailService,IWebHostEnvironment webHostEnvironment,IHttpClientFactory httpClientFactory)
        {
            this.context=context;
            UserRepository = new UserRepositroy(context,mapToUser, scheduleMapper, tokenOptionsModel,mailService,webHostEnvironment,httpClientFactory);

        }
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
