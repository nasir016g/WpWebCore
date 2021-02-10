using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Web.Mvc.About.Models;

namespace Wp.Web.Mvc.About.RestClients
{
    public interface IExperienceWebApi
    {
        [Get("/experience/GetByResumeId/{resumeId}")]
        Task<List<ExperienceModel>> GetExperiences([AliasAs("resumeId")] int resumeId);

        [Get("/experience/{id}")]
        Task<ExperienceModel> GetExperienceById([AliasAs("id")] int id);

        [Put("/experience/{id}")]
        Task Update(int id, ExperienceModel model);

        [Delete("/experience/{id}")]
        Task Delete([AliasAs("id")] int id);

        [Get("/experience/items/{experienceId}")]
        Task<List<ProjectModel>> GetItems(int experienceId);

        [Get("/experience/item/{id}")]
        Task<ProjectModel> GetItem(int id);

        [Put("/experience/item/{id}")]
        Task UpdateItem(int id, ProjectModel model);

        [Delete("/experience/item/{id}")]
        Task DeleteItem([AliasAs("id")] int id);
    }
}
