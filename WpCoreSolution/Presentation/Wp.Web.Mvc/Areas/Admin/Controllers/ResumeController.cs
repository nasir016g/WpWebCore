using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Wp.Core;
using Wp.Core.Domain.Common;
using Wp.Web.Mvc.Models;
using Wp.Web.Mvc.RestClients;

namespace Wp.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
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

            //var cur = _workContext.Current;

            //cur.WorkingLanguageId = 2;
            //_workContext.Current = cur;
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
        public async Task<IActionResult> Edit(ResumeModel model)
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
