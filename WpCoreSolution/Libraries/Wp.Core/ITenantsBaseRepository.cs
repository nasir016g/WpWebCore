using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Wp.Core.Domain.Tenants;

namespace Wp.Core
{
    public interface ITenantsBaseRepository : IBaseRepository<Tenant>
    {
        //T GetById(int id);
        //IQueryable<T> Table { get; }
        //IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        //void Add(T entity);
        //void AddRange(IEnumerable<T> entities);
        //void Remove(T entity);
        //void RemoveRange(IEnumerable<T> entities);
    }



}
