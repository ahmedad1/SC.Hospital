using RepositoryPatternWithUOW.Core.DTOs;
using RepositoryPatternWithUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Interfaces
{
    public  interface IDepartmentRepository 
    {
        public Task<IEnumerable<object>> GetAllAsync(bool includeAllProps);
        public Task AddAsync(DepartmentDto department);
     
    }
}
