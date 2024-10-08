using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.Core.Models
{
    public class UserConnections
    {
        public string? ConnectionId { get; set; }
        public int? UserId { get; set; }
        public virtual User Users { get; set; }
    }
}
