using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Core.Domain.Security;
using Wp.Core.Security;
using Wp.Service.Security;
using Wp.Web.Framework.Models.Admin;

namespace Wp.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SecurityController : BaseAdminController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IClaimProvider _claimProvider;

        public SecurityController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IClaimProvider claimProvider)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _claimProvider = claimProvider;
        }

        #region roles

        
        public ActionResult RoleIndex()
        {
            var roles = _roleManager.Roles.Select(x => x.Name).ToList();

            List<RoleModel> result = new List<RoleModel>();
            foreach(var role in roles)
            {
                var roleModel = new RoleModel();
                roleModel.Name = role;
                roleModel.IsSystemRole = (role == SystemRoleNames.Administrators || role == SystemRoleNames.Editors || role == SystemRoleNames.Users);
                result.Add(roleModel);
            }
            return View(result);
        }        

        public async Task<IActionResult> CreateRole(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await _roleManager.RoleExistsAsync(model.Name))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                    if (result.Succeeded)
                    {
                        SuccessNotification("The role was created successfuly.", true);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ErrorNotification(error.ToString(), true);
                        }
                        return View();
                    }
                }
            }

            return RedirectToAction("RoleIndex");
        }

        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            var result = await _roleManager.DeleteAsync(role);
            return RedirectToAction("RoleIndex");
        }


        #endregion

        #region Users
        public ActionResult UserIndex()
        {
            var model = _userManager.Users.Select(x => new UserModel { Name = x.UserName }).ToList();
            return View(model);
        }

        public ActionResult CreateUser()
        {
            var model = new UserModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateUser(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                user.UserName = model.Name;
                var result = _userManager.CreateAsync(user, model.Password);
                if (result.Result.Succeeded)
                {
                    SuccessNotification("The user was successfully created.", true);
                }
                else
                {
                    foreach (var error in result.Result.Errors)
                    {
                        ErrorNotification(error.ToString(), true);
                    }
                }
            }

            return RedirectToAction("UserIndex");
        }

        public async Task<IActionResult> EditUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var model = new UserModel { Name = userName };

            var roles = _roleManager.Roles;

            foreach (var role in roles)
            {
                var roleModel = new RoleModel { Name = role.Name };
                roleModel.IsChecked = await _userManager.IsInRoleAsync(user, role.Name);
                model.Roles.Add(roleModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var roles = _roleManager.Roles.ToList();

            var user = await _userManager.FindByNameAsync(model.Name);
            foreach (var role in roles)
            {
                if (model.SelectedRoles == null || !model.SelectedRoles.Contains(role.Name))
                {
                    //remove
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        // there must remain at least one admin in system
                        if (role.Name == SystemRoleNames.Administrators)
                        {
                            var adminUsers = await _userManager.GetUsersInRoleAsync(SystemRoleNames.Administrators);
                            if (adminUsers.Count() < 2)
                            {
                                ErrorNotification("Last administrator can't be deleted.", true);
                                return RedirectToAction("EditUser", new { userName = model.Name });
                            }
                        }

                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                        SuccessNotification("User updated successfully.");
                    }
                    
                }
                else
                {
                    //add
                    if (!await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                }
            }         

            SuccessNotification("User updated successfully.");

            return RedirectToAction("EditUser", new { userName = model.Name });
        }

        public async Task<IActionResult> DeleteUser(string userName)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);
            if(roles.Contains(SystemRoleNames.Administrators.ToString()))
            {
                // at least one user must remain in Administrators Role
                // therefore check if the deleting user is the last user 
                var adminUsers = await _userManager.GetUsersInRoleAsync(SystemRoleNames.Administrators);
                if (adminUsers.Count() < 2)
                {
                    ErrorNotification("Last administrator can't be deleted.", true);
                    return RedirectToAction("UserIndex");
                }
            }

            await _userManager.DeleteAsync(user);

            return RedirectToAction("UserIndex");
        }
        #endregion
    }
}
