using System.Collections.Generic;

namespace Wp.Common
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
