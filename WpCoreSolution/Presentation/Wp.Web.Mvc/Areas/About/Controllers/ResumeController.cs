using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nsr.Common.Core;
using System.Threading.Tasks;
using Wp.Core;
using Wp.Web.Mvc.About.Models;
using Wp.Web.Mvc.About.RestClients;
using Wp.Web.Mvc.Helpers;

namespace Wp.Web.Mvc.Areas.About.Controllers
{
    [Area("About")]
    public class ResumeController : Controller
    {
        private readonly IResumesWebApi _resumeManagementApi;
        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private ResiliencyHelper _resiliencyHelper;

        public ResumeController(IResumesWebApi resumeManagementApi, ILogger<ResumeController> logger, IWorkContext workContext)
        {
            _resumeManagementApi = resumeManagementApi;
            _logger = logger;
            _workContext = workContext;
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

       
        public async Task<IActionResult> Edit(int id)
        {
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {

                var model = await _resumeManagementApi.GetResumeById(id);

                return View(model);
            }, View("Offline"));
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ResumeAdminModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            {
                await _resumeManagementApi.Update(model.Id, model);
                return RedirectToAction("Index");
            }, View("Offline"));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            {
                await _resumeManagementApi.Delete(id);
                return RedirectToAction("Index");
            }, View("Offline"));
        }

    }
}
