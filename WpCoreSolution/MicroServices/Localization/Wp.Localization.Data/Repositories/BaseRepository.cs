using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Wp.Common;



namespace Wp.Localization.Data.Repositories
{
    public interface ILocalizationBaseRepository<T> where T : Entity
    {
        IQueryable<T> Table { get; }

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T GetById(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
    public class LocalizationBaseRepository<T> : ILocalizationBaseRepository<T> where T : Entity
    {
        protected readonly WpLocalizationDbContext Context;

        public LocalizationBaseRepository(WpLocalizationDbContext context)
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
