using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Web.Mvc.About.Models;

namespace Wp.Web.Mvc.About.RestClients
{
    public interface IEducationWebApi
    {
        [Get("/education/GetByResumeId/{resumeId}")]
        Task<List<EducationModel>> GetEducations([AliasAs("resumeId")] int resumeId);

        [Get("/education/{id}")]
        Task<EducationModel> GetEducationById([AliasAs("id")] int id);

        [Put("/education/{id}")]        
        Task Update(int id, EducationModel model);

        [Delete("/education/{id}")]
        Task Delete([AliasAs("id")] int id);

        [Get("/education/items/{educationId}")]
        Task<List<EducationItemModel>> GetItems(int educationId);

        [Get("/education/item/{id}")]
        Task<EducationItemModel> GetItem(int id);

        [Put("/education/item/{id}")]
        Task UpdateItem(int id, EducationItemModel model);

        [Delete("/education/item/{id}")]
        Task DeleteItem([AliasAs("id")] int id);
    }
}
