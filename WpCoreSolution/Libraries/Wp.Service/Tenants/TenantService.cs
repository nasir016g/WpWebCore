using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Core;
using Wp.Core.Domain.Tenants;
using Wp.Data;

namespace Wp.Service.Tenants
{
    public class TenantService : TenantEntityService, ITenantService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITenantsBaseRepository _tenantRepo;
        private List<Tenant> _tenants;
        private Tenant _currentTenant;

        public TenantService(
            IHttpContextAccessor httpContextAccessor,
            TenantDbContext dbContext,
            ITenantsBaseRepository repository) : base(dbContext, repository)
        {

            _httpContextAccessor = httpContextAccessor;
            _tenantRepo = repository;
            _tenants = _tenantRepo.Table.ToList();

            if (_httpContextAccessor.HttpContext != null)
            {
                _currentTenant = GetTenantByName();
            }
            //else
            //{
            //    var tenantid = new Guid("10DEE2B7-DCBA-45E6-8E03-380D27772944"); // default
            //    _currentTenant = GetTenantByTenantId(tenantid);
            //}

        }

        #region Utilities

        private Tenant GetTenantByName()
        {
            if (_tenants == null)
            {
                _tenants = _tenantRepo.Table.ToList();
            }

            Tenant tenant = null;

            string tenantName = _httpContextAccessor.HttpContext?.Request?.Headers?["Tenant"];
            if (tenantName == null)
            {
                // query string (install controller)
                var path = _httpContextAccessor.HttpContext.Request.Path;
                if (path.HasValue)
                {
                    tenantName = _httpContextAccessor.HttpContext.Request.Query["Name"];
                    if (tenantName == null)
                        tenantName = "WpCore1"; // default
                }
            }

            tenant = _tenants.FirstOrDefault(t => t.TenantName.ToLowerInvariant() == tenantName.ToLowerInvariant());
            return tenant;
        }

        private Tenant GetTenantByTenantId(Guid TenantId)
        {
            if (_tenants == null)
            {
                _tenants = _tenantRepo.Table.ToList();
            }
            var tenant = _tenants.FirstOrDefault(t => t.TenantId == TenantId);
            return tenant;
        }

        #endregion


        public Tenant GetTenant()
        {
            return _currentTenant;
        }

        public void InstallTenants()
        {
            if (this.GetAll().Count > 0) return;

            var tenants = new List<Tenant>()
            {
                //new Tenant { TenantId = Guid.Parse("10DEE2B7-DCBA-45E6-8E03-380D27772944"), TenantName = "WpCore1", ConnectionString = @"server=.\sqlexpress;user id=sa;pwd=aq;persist security info=False;initial catalog=WpCore1;Integrated security=false;Trusted_Connection=false;MultipleActiveResultSets=true"},
                //new Tenant { TenantId = Guid.Parse("66616AEF-53B4-45D8-A9CE-7E6A5CED7EF3"), TenantName = "WpCore2", ConnectionString = @"server=.\sqlexpress;user id=sa;pwd=aq;persist security info=False;initial catalog=WpCore2;Integrated security=false;Trusted_Connection=false;MultipleActiveResultSets=true"},

                new Tenant { TenantId = Guid.Parse("10DEE2B7-DCBA-45E6-8E03-380D27772944"), TenantName = "WpCore1", ConnectionString = @"Filename=./WpCore1.sqlite"},
                new Tenant { TenantId = Guid.Parse("66616AEF-53B4-45D8-A9CE-7E6A5CED7EF3"), TenantName = "WpCore2", ConnectionString = @"Filename=WpCore2.sqlite"},
            };

            tenants.ForEach(t => Insert(t));
        }
    }
}
