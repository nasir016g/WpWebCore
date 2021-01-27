using Nsr.Common.Core;
using Nsr.Common.Data;
using Nsr.Common.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nsr.Common.Services
{
    public abstract class EntityService<T> : IEntityService<T> where T : Entity
    {
        protected ICommonUnitOfWork _unitOfWork;
        ICommonBaseRepository<T> _repository;

        public EntityService(ICommonUnitOfWork unitOfWork, ICommonBaseRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
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
            _unitOfWork.Complete();

        }


        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            
            _unitOfWork.Complete();
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Remove(entity);
            _unitOfWork.Complete();

        }

        public virtual IList<T> GetAll()
        {
            return _repository.Table.ToList();
        }
    }
}
