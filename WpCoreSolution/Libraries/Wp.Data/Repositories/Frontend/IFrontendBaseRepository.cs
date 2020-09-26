using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Wp.Core.Domain.Frontend;

namespace Wp.Data.Repositories.Frontend

{
    public interface IFrontendBaseRepository<T> where T : FrontendEntity
    {
        T GetById(int id);
        IQueryable<T> Table { get; }
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
