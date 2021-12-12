using System.Collections.Generic;
using Nsr.Common.Core;
using Nsr.Wh.Web.Domain;

namespace Nsr.Wh.Web.Services
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