using System;
using System.Collections.Generic;
using Wp.Service.Security;

namespace Wp.Web.Api.Models.Admin
{
    public class ClaimRoleModel
    {
        public ClaimRoleModel()
        {
            AvailableClaims = new List<ClaimRecord>();
            AvailableRoles = new List<RoleModel>();
            Allowed = new Dictionary<string, IDictionary<string, bool>>(StringComparer.OrdinalIgnoreCase);
        }

        public IList<ClaimRecord> AvailableClaims { get; set; }
        public IList<RoleModel> AvailableRoles { get; set; }
        public IDictionary<string, IDictionary<string, bool>> Allowed { get; set; }
    }


}
