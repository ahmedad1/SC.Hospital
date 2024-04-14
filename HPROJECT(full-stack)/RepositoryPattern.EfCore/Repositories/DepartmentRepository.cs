using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RepositoryPattern.EfCore;
using RepositoryPatternWithUOW.Core.DTOs;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.EfCore.MapToModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EfCore.Repositories
{
    public class DepartmentRepository(AppDbContext context,MapToDepartment mapper) : IDepartmentRepository
    {
        private async Task<byte[]> GetFileAsBinary(IFormFile file)
        {
            using MemoryStream stream = new MemoryStream();
            await file.CopyToAsync(stream);
            return stream.ToArray();
        }
        public async Task AddAsync(DepartmentDto department)
        {
            var dept=mapper.MapToDept(department);
            dept.BackgroundCardImage =await GetFileAsBinary(department.BackgroundCardImage);
            await context.Set<Department>().AddAsync(dept);
            
        }

        public async Task<IEnumerable<object>> GetAllAsync(bool includeAllProps)
        {
            context.ChangeTracker.LazyLoadingEnabled =false;
            if (includeAllProps)
                return await context.Set<Department>().AsNoTracking().ToListAsync();
            return await context.Set<Department>().AsNoTracking().Select(x => new {x.Id,x.DepartmentName}).ToListAsync();
           
        }

    }
}
