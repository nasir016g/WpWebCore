using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wp.Web.Mvc.Models;

namespace Wp.Web.Mvc.RestClients
{
    public interface IResumesWebApi
    {
        [Get("/resume")]
        Task<List<ResumeModel>> GetResume();

        [Get("/resume/{id}")]
        Task<ResumeModel> GetResumeById([AliasAs("id")] int id);

        [Put("/resume/{id}")]
        Task Update(int id, ResumeModel model);

        [Delete("/resume/{id}")]
        Task Delete([AliasAs("id")] int id);

    }
}
