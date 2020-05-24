using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Wp.Core;
using Wp.Core.Domain.Tenants;

namespace Wp.Data.Repositories
{
    public class TenantsBaseRepository : ITenantsBaseRepository
    {
        protected readonly TenantDbContext Context;

        public TenantsBaseRepository(TenantDbContext context)
        {
            Context = context;
        }

        public virtual Tenant GetById(int id)
        {
            return Context.Tenants.Find(id);
        }

        public virtual IQueryable<Tenant> Table
        {
            get
            {
                return Context.Tenants;
            }
        }

        public virtual IEnumerable<Tenant> Find(Expression<Func<Tenant, bool>> predicate)
        {
            return Context.Tenants.Where(predicate);
        }

        public void Add(Tenant entity)
        {
            Context.Tenants.Add(entity);
        }

        public void AddRange(IEnumerable<Tenant> entitiesToAdd)
        {
            Context.Tenants.AddRange(entitiesToAdd);
        }

        public void Remove(Tenant entity)
        {
            Context.Tenants.Remove(entity);
        }

        public void RemoveRange(IEnumerable<Tenant> entitiesToRemove)
        {
            Context.Tenants.RemoveRange(entitiesToRemove);
        }
    }
}
