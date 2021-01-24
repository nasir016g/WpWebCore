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
    }
}
