using System;
using System.Collections.Generic;
using System.Text;

namespace Nsr.Common.Core
{
    public interface IBaseService<T> where T : Entity
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Create(T entity);
        void Delete(T entity);
        void Update(T entity);
    }
}
