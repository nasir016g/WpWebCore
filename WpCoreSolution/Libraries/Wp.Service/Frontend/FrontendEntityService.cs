using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Core;
using Wp.Core.Domain.Frontend;
using Wp.Data;
using Wp.Data.Repositories.Frontend;

namespace Wp.Services.Frontend
{
    public abstract class FrontendEntityService<T, SourceT> : IFrontendEntityService<T, SourceT> 
        where T : FrontendEntity
        where SourceT: Entity
    {
        private readonly FrontendDbContext _frontendDbContext;
        protected IFrontendBaseRepository<T> _repository;

        public FrontendEntityService(FrontendDbContext frontendDbContext, IFrontendBaseRepository<T> repository)
        {
            _frontendDbContext = frontendDbContext;
            _repository = repository;
        }

        public virtual T GetById(int id)
        {
            return _repository.GetById(id);
        }



        public virtual void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _repository.Add(entity);
            _frontendDbContext.SaveChanges();

        }


        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _frontendDbContext.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Remove(entity);
            _frontendDbContext.SaveChanges();
        }

        public virtual IList<T> GetAll()
        {
            return _repository.Table.ToList();
        }

        public abstract void Insert(SourceT source);
        public abstract void Delete(SourceT source);
        public abstract void Update(SourceT source);
    }
}
