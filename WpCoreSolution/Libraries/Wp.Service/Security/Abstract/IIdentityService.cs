using System;
using System.Collections.Generic;
using System.Text;
using Wp.Core.Security;

namespace Wp.Service.Security
{
    public interface IIdentityService
    {
       // UserManager<ApplicationUser> UserManager { get; set; }
       // RoleManager<IdentityRole> RoleManager { get; set; }
        IList<string> GetRoles();
        void DeleteRole(string roleName);

        IList<ApplicationUser> GetUsers();
        bool CanDeleteAdmin();
        void DeleteUser(ApplicationUser user);

    }
}
