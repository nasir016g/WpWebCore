using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Core.Domain.Security;
using Wp.Core.Security;

namespace Wp.Service.Security
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void DeleteRole(string roleName)
        {
            var role = _roleManager.FindByNameAsync(roleName).Result;            
           var result = _roleManager.DeleteAsync(role).Result;
            if(!result.Succeeded)
            {
                result.Errors.First().ToString();
            }
        }

        public void DeleteUser(ApplicationUser user)
        {
           var result = _userManager.DeleteAsync(user).Result;
            if(!result.Succeeded)
            {
                result.Errors.First().ToString();
            }           
        }

        public IList<string> GetRoles()
        {
           return _roleManager.Roles.Select(x => x.Name).ToList();
        }

        public IList<ApplicationUser> GetUsers()
        {
            return _userManager.Users.ToList();
        }

        public bool CanDeleteAdmin()
        {
            // at least one user must remain in Administrators Role
            // therefore check if the deleting user is the last user 
            return (_userManager.GetUsersInRoleAsync(SystemRoleNames.Administrators).Result.Count() > 1);
        }
    }
}
