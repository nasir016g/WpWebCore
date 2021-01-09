using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wp.Web.Mvc.Areas.Admin.Models;

namespace Wp.Web.Mvc.Areas.Admin.RESTClients
{
    public interface IResumeManagementApi
    {
        [Get("/resume")]
        Task<List<ResumeModel>> GetResume();
    }
}
