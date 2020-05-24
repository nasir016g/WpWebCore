using Microsoft.AspNetCore.Identity;
using Wp.Core.Domain.Career;

namespace Wp.Core.Security
{
    public class ApplicationUser : IdentityUser
    {        
        public int? ResumeId { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
