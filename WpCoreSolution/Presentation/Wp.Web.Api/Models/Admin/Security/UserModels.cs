using System.Collections.Generic;

namespace Wp.Web.Api.Models.Admin
{
    public class UserModel
    {
        public UserModel()
        {
            Roles = new List<RoleModel>();
        }
        public string Name { get; set; }
        public string Password { get; set; }

        public IList<RoleModel> Roles { get; set; }
        //public IEnumerable<string> SelectedRoles { get; set; } //
    }
}
