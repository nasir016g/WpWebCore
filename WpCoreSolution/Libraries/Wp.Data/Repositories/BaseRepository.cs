using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Wp.Core;


namespace Wp.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        protected readonly WpDbContext Context;

        public BaseRepository(WpDbContext context)
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
        }

        public void AddRange(IEnumerable<T> entitiesToAdd)
        {
            Context.Set<T>().AddRange(entitiesToAdd);
        }

        public void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entitiesToRemove)
        {
            Context.Set<T>().RemoveRange(entitiesToRemove);
        }
    }
}
