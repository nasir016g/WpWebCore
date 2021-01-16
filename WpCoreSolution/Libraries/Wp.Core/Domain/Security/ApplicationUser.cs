using Microsoft.AspNetCore.Identity;

namespace Wp.Core.Security
{
    public class ApplicationUser : IdentityUser
    {        
        public int? ResumeId { get; set; }
        //public virtual Resume Resume { get; set; }
    }
}
