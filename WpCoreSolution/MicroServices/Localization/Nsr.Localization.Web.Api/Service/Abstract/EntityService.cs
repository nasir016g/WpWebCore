using Nsr.Common.Core;
using Nsr.Common.Data;
using Nsr.Common.Data.Repositories;
using Nsr.Localization.Web.Api.Data;
using Nsr.Localization.Web.Api.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nsr.Localization.Web.Api.Services
{
    public abstract class EntityService<T> : IEntityService<T> where T : Entity
    {
        protected ILocalizationUnitOfWork _unitOfWork;
        ILocalizationBaseRepository<T> _repository;

        public EntityService(ILocalizationUnitOfWork unitOfWork, ILocalizationBaseRepository<T> repository)
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
