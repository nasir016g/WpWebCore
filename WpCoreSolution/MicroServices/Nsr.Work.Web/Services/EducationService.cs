using System.Collections.Generic;
using System.Linq;
using Nsr.Common.Core;
using Nsr.Work.Web.Domain;

namespace Nsr.Work.Web.Services
{


    public class EducationService : EntityService<Education>, IEducationService
    {
        private IBaseRepository<Education> _repository;
        private IBaseRepository<EducationItem> _educationItemRepo;

        public EducationService(IUnitOfWork unitOfWork, 
            IBaseRepository<Education> repository,
            IBaseRepository<EducationItem> educationItemRepo) : base(unitOfWork, repository)
        {
            _repository = repository;
            _educationItemRepo = educationItemRepo;
        }


        #region Edu
        public IList<Education> GetAll(int ResumeId)
        {
            return _repository.Table.Where(x => x.ResumeId == ResumeId).ToList();
        }

        #endregion

        #region Item
        public IList<EducationItem> GetEducationItemsByEducationId(int educationId)
        {
           return _educationItemRepo.Table.Where(x => x.EducationId == educationId).ToList();
        }

        public EducationItem GetEducationItemById(int id)
        {
            return _educationItemRepo.GetById(id);
        }

        public void InsertEducationItem(EducationItem t)
        {
            _educationItemRepo.Add(t);
        }

        public void UpdateEducationItem(EducationItem t)
        {
            _unitOfWork.Complete();
        }

        public void DeleteEducationItem(EducationItem t)
        {
            _educationItemRepo.Remove(t);
            _unitOfWork.Complete();
        }
        #endregion
    }
}
