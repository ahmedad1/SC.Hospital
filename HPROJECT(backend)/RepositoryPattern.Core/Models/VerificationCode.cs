using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.Core.Models
{
    public class VerificationCode
    {
        public int Code { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
