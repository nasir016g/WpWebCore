using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Web.Mvc.Profile.Models;

namespace Wp.Web.Mvc.Profile.RestClients
{
    public interface IExperienceWebApi
    {
        [Get("/experience/GetByResumeId/{resumeId}")]
        Task<List<ExperienceAdminModel>> GetExperiences([AliasAs("resumeId")] int resumeId);

        [Get("/experience/{id}")]
        Task<ExperienceAdminModel> GetExperienceById([AliasAs("id")] int id);

        [Put("/experience/{id}")]
        Task Update(int id, ExperienceAdminModel model);

        [Delete("/experience/{id}")]
        Task Delete([AliasAs("id")] int id);

        [Get("/experience/items/{experienceId}")]
        Task<List<ProjectAdminModel>> GetItems(int experienceId);

        [Get("/experience/item/{id}")]
        Task<ProjectAdminModel> GetItem(int id);

        [Put("/experience/item/{id}")]
        Task UpdateItem(int id, ProjectAdminModel model);

        [Delete("/experience/item/{id}")]
        Task DeleteItem([AliasAs("id")] int id);
    }
}
