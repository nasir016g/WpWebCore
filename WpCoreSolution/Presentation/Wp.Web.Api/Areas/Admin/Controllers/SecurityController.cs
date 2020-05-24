using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Core.Domain.Security;
using Wp.Core.Security;
using Wp.Service.Security;
using Wp.Web.Api.Models.Admin;

namespace Wp.Web.Api.Areas.Admin.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
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
            this._claimProvider = claimProvider;
        }

        #region roles

        // GET: api/Security
        [HttpGet("Roles", Name = "Roles")]
        public IEnumerable<RoleModel> Roles()
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
            return result;
        }

        // POST: api/Security
        [HttpPost("Roles/{roleName}")]
        public async Task<IActionResult> Post(string roleName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest("Role already exists.");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            return Ok(result);
        }

        // PUT: api/Security/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("Roles/{roleName}")]
        public async Task<IActionResult> Delete(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            var result = await _roleManager.DeleteAsync(role);
            return NoContent();
        }

        #endregion

        #region users

        [HttpGet("Users", Name = "Users")]
        public IEnumerable<UserModel> Users()
        {
            var users = _userManager.Users.Select(x => new UserModel { Name = x.UserName }).ToList();
            return users;
        }

        [HttpGet("Users/{userName}")]
        public async Task<IActionResult> GetUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var model = new UserModel { Name = userName };
            //var roles = _roleManager.Roles.Select(x => new RoleModel { Name = x.Name});
            var roles = _roleManager.Roles;

            foreach (var role in roles)
            {
                var roleModel = new RoleModel { Name = role.Name };
                roleModel.IsChecked = await _userManager.IsInRoleAsync(user, role.Name);
                model.Roles.Add(roleModel);
            }

            return Ok(model);
        }

        [HttpPost("users")]
        public async Task<IActionResult> PostUser([FromBody]UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.Name);
            foreach (var role in model.Roles)
            {
                if (role.IsChecked)
                {
                    //add
                    if (!await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                }
                else
                {
                    //remove
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        // there must remain at least one admin in system
                        if (role.Name == SystemRoleNames.Administrators)
                        {
                            var adminUsers = await _userManager.GetUsersInRoleAsync(SystemRoleNames.Administrators);
                            if (adminUsers.Count() < 2) continue;
                        }

                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                }
            }

            return NoContent();
        }

        //[HttpPut("users")]
        //public IActionResult PutUser(UserModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    return NoContent();
        //}
        #endregion

        #region claims

        [HttpGet("claims")]
        public async Task<IActionResult> GetClaims()
        {
            var model = new ClaimRoleModel();

            var claims = _claimProvider.GetClaims();
            var roles = _roleManager.Roles;

            model.AvailableClaims = claims.ToList();

            foreach (var r in roles)
            {
                model.AvailableRoles.Add(new RoleModel
                {
                    Name = r.Name
                });
            }


            foreach (var r in roles)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(r);
                foreach (var c in claims)
                {
                    var allowed = roleClaims.Any(rc => rc.Value == c.ClaimValue);
                    if (!model.Allowed.ContainsKey(c.ClaimValue))
                        model.Allowed[c.ClaimValue] = new Dictionary<string, bool>();
                    model.Allowed[c.ClaimValue][r.Name] = allowed;
                }
            }

            return Ok(model);
        }

        [HttpPut("claims")]
        public async Task<IActionResult> SaveClaims(ClaimRoleModel model)
        {
            var claims = _claimProvider.GetClaims();
            var roles = _roleManager.Roles;

            foreach(var r in roles)
            {
                var roleName = r.Name.First().ToString().ToLower() + r.Name.Substring(1); // first char lowercase
                var roleClaims = await _roleManager.GetClaimsAsync(r);
                foreach (var c in claims)
                {
                    var allow = model.Allowed[c.ClaimValue][roleName];
                    if(allow)
                    {
                        if (roleClaims.FirstOrDefault(x => x.Value == c.ClaimValue) == null)
                            await _roleManager.AddClaimAsync(r, new System.Security.Claims.Claim(c.ClaimType, c.ClaimValue));
                    }
                    else
                    {
                        await _roleManager.RemoveClaimAsync(r, new System.Security.Claims.Claim(c.ClaimType, c.ClaimValue));
                    }
                }
            }
            foreach (var cr in  model.Allowed)
            {
                
            }
            return NoContent();
        }

        #endregion
    }
}
