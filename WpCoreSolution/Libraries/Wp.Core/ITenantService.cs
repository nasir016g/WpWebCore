using System;
using System.Collections.Generic;
using System.Text;
using Nsr.Common.Core;
using Wp.Core.Domain.Tenants;

namespace Wp.Core
{
    public interface ITenantService : IEntityService<Tenant>
    {
        Tenant GetTenant();

        void InstallTenants();
    }
}
