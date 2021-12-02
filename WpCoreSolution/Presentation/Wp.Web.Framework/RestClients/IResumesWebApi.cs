using Nsr.Common.Core.Models;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wp.Web.Framework.Models.WorkHistories;

namespace Wp.Web.Framework.RestClients
{
    public interface IResumesWebApi
    {
        [Get("/resume")]
        Task<List<ResumeModel>> GetResume();

        [Get("/resume/{id}")]
        Task<ResumeModel> GetResumeById([AliasAs("id")] int id);        

        [Put("/resume/{id}")]
        //[Put("/resume")]
        Task Update(int id, ResumeModel model);

        [Delete("/resume/{id}")]
        Task Delete([AliasAs("id")] int id);

        [Get("/resume/details/{id}")]
        Task<WorkHistoryDetailsModel> GetResumeDetails([AliasAs("id")] int id);

    }
}
