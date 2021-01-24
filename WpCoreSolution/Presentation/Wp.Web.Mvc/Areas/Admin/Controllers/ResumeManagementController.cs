using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Wp.Web.Mvc.RestClients;

namespace Wp.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ResumeManagementController : Controller
    {
        private readonly IResumesWebApi _resumeManagementApi;
        private readonly ILogger _logger;
        private ResiliencyHelper _resiliencyHelper;

        public ResumeManagementController(IResumesWebApi resumeManagementApi, ILogger<ResumeManagementController> logger)
        {
            _resumeManagementApi = resumeManagementApi;
            _logger = logger;
            _resiliencyHelper = new ResiliencyHelper(_logger);
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {
                var model = await _resumeManagementApi.GetResume();
                
                return View(model);
            }, View("Offline"));
        }

    }
}
