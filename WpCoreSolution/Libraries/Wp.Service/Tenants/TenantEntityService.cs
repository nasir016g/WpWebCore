using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Core;
using Wp.Core.Domain.Tenants;
using Wp.Data;
using Wp.Services;

namespace Wp.Service.Tenants
{
    public abstract class TenantEntityService : IEntityService<Tenant> 
    {
        protected TenantDbContext _dbContext;
        ITenantsBaseRepository _repository;

        public TenantEntityService(TenantDbContext dbContext, ITenantsBaseRepository repository)
        {
            _dbContext = dbContext;
            _repository = repository;
        }

        public virtual Tenant GetById(int id)
        {
            return _repository.GetById(id);
        }

        public virtual void Insert(Tenant entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _repository.Add(entity);
            _dbContext.SaveChanges();
        }

        public virtual void Update(Tenant entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _dbContext.SaveChanges();
        }

        public virtual void Delete(Tenant entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Remove(entity);
            _dbContext.SaveChanges();
        }

        public virtual IList<Tenant> GetAll()
        {
            return _repository.Table.ToList();
        }
    }
}
