using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Wp.Core.Domain.Security;

namespace Wp.Service.Security
{
    public class StandardClaimProvider : IClaimProvider
    {
        public static readonly ClaimRecord ViewWebPages = new ClaimRecord { ClaimType = "WebPage", ClaimValue = "View web pages" };
        public static readonly ClaimRecord CreateWebPages = new ClaimRecord { ClaimType = "WebPage", ClaimValue = "Create web pages" };
        public static readonly ClaimRecord EditWebPages = new ClaimRecord { ClaimType = "WebPage", ClaimValue = "Edit web pages" };
        public static readonly ClaimRecord EditOthersWebpages = new ClaimRecord { ClaimType = "WebPage", ClaimValue = "Edit others web pages" };

        public static readonly ClaimRecord ManageRoles = new ClaimRecord { ClaimType = "Role", ClaimValue = "Manage roles" };
        public static readonly ClaimRecord ManageUsers = new ClaimRecord { ClaimType = "User", ClaimValue = "Manage users" };
        public static readonly ClaimRecord Export = new ClaimRecord { ClaimType = "", ClaimValue = "Export" };
        public static readonly ClaimRecord Import = new ClaimRecord { ClaimType = "", ClaimValue = "Import" };




        public virtual IEnumerable<ClaimRecord> GetClaims()
        {
            return new[]
            {
                ViewWebPages,
                EditWebPages,
                CreateWebPages,
                ManageRoles,
                ManageUsers
            };
        }

        public virtual IEnumerable<DefaultClaimRecord> GetDefaultClaims()
        {

            // editors
            // authors 
            return new[]
            {
                new DefaultClaimRecord
                {
                    RoleName = SystemRoleNames.Administrators,
                    ClaimRecords = new[]
                    {
                        ViewWebPages,
                        EditWebPages,
                        CreateWebPages,
                        ManageRoles,
                        ManageUsers
                    }
                },
                new DefaultClaimRecord
                {
                    RoleName = SystemRoleNames.Users,
                    ClaimRecords = new[]
                    {
                        ViewWebPages
                    }
                }
            };
        }
    }
}
