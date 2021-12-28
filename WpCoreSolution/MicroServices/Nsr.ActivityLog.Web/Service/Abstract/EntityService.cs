using Nsr.ActivityLogs.Web.Data;
using Nsr.ActivityLogs.Web.Data.Repositories;
using Nsr.Common.Core;

namespace Nsr.ActivityLogs.Web.Service.Abstract
{
    public abstract class EntityService<T> : IEntityService<T> where T : Entity
    {
        protected IActivityLogUnitOfWork _unitOfWork;
        protected IActivityLogBaseRepository<T> _repository;

        public EntityService(IActivityLogUnitOfWork unitOfWork, IActivityLogBaseRepository<T> repository)
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
