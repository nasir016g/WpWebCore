using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Wp.Core.Domain.Frontend;

namespace Wp.Data.Repositories.Frontend
{
    public class FrontendBaseRepository<T> : IFrontendBaseRepository<T> where T : FrontendEntity
    {
        protected readonly FrontendDbContext Context;

        public FrontendBaseRepository(FrontendDbContext context)
        {
            Context = context;
        }

        public virtual T GetById(int id)
        {
            return Context.Set<T>().Find(id);
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return Context.Set<T>();
            }
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().Where(predicate);
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();
        }

        public void AddRange(IEnumerable<T> entitiesToAdd)
        {
            Context.Set<T>().AddRange(entitiesToAdd);
            Context.SaveChanges();
        }

        public void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
            Context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<T> entitiesToRemove)
        {
            Context.Set<T>().RemoveRange(entitiesToRemove);
            Context.SaveChanges();
        }
    }
}
