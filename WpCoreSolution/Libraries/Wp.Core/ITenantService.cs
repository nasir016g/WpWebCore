using System;
using System.Collections.Generic;
using System.Text;
using Wp.Core.Domain.Tenants;

namespace Wp.Core
{
    public interface ITenantService : IEntityService<Tenant>
    {
        Tenant GetTenant();

        void InstallTenants();
    }
}
