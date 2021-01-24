using System.Collections.Generic;
using Wp.Common;
using Wp.Resumes.Core.Domain;

namespace Wp.Resumes.Services
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