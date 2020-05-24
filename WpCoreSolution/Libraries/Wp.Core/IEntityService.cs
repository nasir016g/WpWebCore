using System.Collections.Generic;
using Wp.Core;

namespace Wp.Core
{
    public interface IEntityService<T> : IService
    where T : EntityBase
    {
        T GetById(int id);
        void Insert(T entity);
        void Delete(T entity);
        IList<T> GetAll();
        void Update(T entity);
    }
}
