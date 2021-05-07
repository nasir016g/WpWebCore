using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Web.Mvc.Profile.Models;

namespace Wp.Web.Mvc.Profile.RestClients
{
    public interface IEducationWebApi
    {
        [Get("/education/GetByResumeId/{resumeId}")]
        Task<List<EducationAdminModel>> GetEducations([AliasAs("resumeId")] int resumeId);

        [Get("/education/{id}")]
        Task<EducationAdminModel> GetEducationById([AliasAs("id")] int id);

        [Put("/education/{id}")]        
        Task Update(int id, EducationAdminModel model);

        [Delete("/education/{id}")]
        Task Delete([AliasAs("id")] int id);

        [Get("/education/items/{educationId}")]
        Task<List<EducationItemAdminModel>> GetItems(int educationId);

        [Get("/education/item/{id}")]
        Task<EducationItemAdminModel> GetItem(int id);

        [Put("/education/item/{id}")]
        Task UpdateItem(int id, EducationItemAdminModel model);

        [Delete("/education/item/{id}")]
        Task DeleteItem([AliasAs("id")] int id);
    }
}
