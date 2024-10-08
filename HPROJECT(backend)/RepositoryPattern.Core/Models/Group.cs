using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.Core.Models
{
    public class Group
    {
        public string? GroupsName { get; set; }
        public virtual ICollection<User>? Users { get; set; } 
    


    }
}
