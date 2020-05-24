using System.Collections.Generic;
using System.Linq;
using Wp.Core;
using Wp.Core.Domain.Career;
using Wp.Data;

namespace Wp.Services.Career
{
    public class ExperienceService : EntityService<Experience>, IExperienceService
    {
        private IBaseRepository<Experience> _workExperienceRepo;
        private IBaseRepository<Project> _projectRepo;

        public ExperienceService(IUnitOfWork unitOfWork, IBaseRepository<Experience> workExperienceRepo, IBaseRepository<Project> projectRepo)
        : base(unitOfWork, workExperienceRepo)
        {
            this._workExperienceRepo = workExperienceRepo;
            this._projectRepo = projectRepo;
        }

        #region WE
        public IList<Experience> GetAll(int ResumeId)
        {
            return _workExperienceRepo.Table.Where(x => x.ResumeId == ResumeId).OrderByDescending(x => x.DisplayOrder).ToList();
        }

        #endregion

        #region Item
        public IList<Project> GetProjectsByExperienceId(int experienceId)
        {
            return _projectRepo.Table.Where(x => x.ExperienceId == experienceId).ToList();
        }

        public Project GetProjectById(int id)
        {
            return _projectRepo.GetById(id);
        }

        public void InsertProject(Project t)
        {
            _projectRepo.Add(t);
        }

        public void UpdateProject(Project t)
        {
            _unitOfWork.Complete();
        }

        public void DeleteProject(Project t)
        {
            _projectRepo.Remove(t);
            _unitOfWork.Complete();

        }
        #endregion
    }
}
