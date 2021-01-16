using System;
using System.Collections.Generic;
using System.Linq;
using Wp.Common;
using Wp.Services.Events;

namespace Wp.Services
{
    public abstract class EntityService<T> : IEntityService<T> where T : Entity
    {
        protected IUnitOfWork _unitOfWork;
        IBaseRepository<T> _repository;
        private readonly IEventPublisher _eventPublisher;

        public EntityService(IUnitOfWork unitOfWork, IBaseRepository<T> repository, IEventPublisher eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _eventPublisher = eventPublisher;
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
            _eventPublisher.EntityInserted(entity);

        }


        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            
            _unitOfWork.Complete();
            _eventPublisher.EntityUpdated(entity);
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Remove(entity);
            _unitOfWork.Complete();
            _eventPublisher.EntityDeleted(entity);

        }

        public virtual IList<T> GetAll()
        {
            return _repository.Table.ToList();
        }
    }
}
