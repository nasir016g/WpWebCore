using Nsr.Common.Core;
using System.Linq.Expressions;

namespace Nsr.ActivityLogs.Web.Data.Repositories
{
    public interface IActivityLogBaseRepository<T> where T : Entity
    {
        IQueryable<T> Table { get; }

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T GetById(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
    public class ActivityLogBaseRepository<T> : IActivityLogBaseRepository<T> where T : Entity
    {
        protected readonly ActivityLogDbContext Context;

        public ActivityLogBaseRepository(ActivityLogDbContext context)
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
