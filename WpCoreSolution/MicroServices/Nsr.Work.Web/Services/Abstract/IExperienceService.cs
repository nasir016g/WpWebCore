using System.Collections.Generic;
using Nsr.Common.Core;
using Nsr.Work.Web.Domain;

namespace Nsr.Work.Web.Services
{
    public interface IExperienceService : IEntityService<Experience>
    {
        IList<Experience> GetAll(int ResumeId);

        IList<Project> GetProjectsByExperienceId(int skillId);
        Project GetProjectById(int id);
        void InsertProject(Project t);
        void UpdateProject(Project t);
        void DeleteProject(Project t);
    }
}