using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.Core.Models
{
    public class RefreshToken
    {

        public int Id { get; set; }    
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsActive => ExpiresAt < DateTime.Now;
        public int UserId { get; set; }
        public virtual User User { get; set; }

       
    }
}
