using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nsr.Common.Core;
using Nsr.Common.Core.Models;
using System.Threading.Tasks;
using Wp.Web.Framework.RestClients;
using Wp.Web.Mvc.Helpers;

namespace Wp.Web.Mvc.Areas.Profile.Controllers
{
    [Area("Profile")]
    public class EducationController : Controller
    {
        private readonly IEducationWebApi _educationWebApi;
        private readonly ILogger<EducationController> _logger;
        private readonly IWorkContext _workContext;
        private ResiliencyHelper _resiliencyHelper;

        public EducationController(IEducationWebApi educationWebApi, ILogger<EducationController> logger, IWorkContext workContext)
        {
            _educationWebApi = educationWebApi;
            _logger = logger;
            _workContext = workContext;
            _resiliencyHelper = new ResiliencyHelper(_logger);
        }

        #region Education
        public async Task<IActionResult> Index(int resumeId)
        {
            ViewBag.ResumeId = resumeId;
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {
                var model = await _educationWebApi.GetEducations(resumeId);

                return View(model);
            }, View("Offline"));
        }


        public async Task<IActionResult> Edit(int id)
        {
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {

                var model = await _educationWebApi.GetEducationById(id);

                return View(model);
            }, View("Offline"));
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EducationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            {
                await _educationWebApi.Update(model.Id, model);
                return RedirectToAction("Index");
            }, View("Offline"));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            {
                await _educationWebApi.Delete(id);
                return RedirectToAction("Index");
            }, View("Offline"));
        }

        #endregion

        #region Item

        public async Task<IActionResult> ItemIndex(int educationId, int resumeId)
        {
            ViewBag.ResumeId = resumeId;
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {
                var model = await _educationWebApi.GetItems(educationId);
                
                return View(model);
            }, View("Offline"));
        }


        public async Task<IActionResult> ItemEdit(int id, int resumeId)
        {

            return await _resiliencyHelper.ExecuteResilient(async () =>
            {

                var model = await _educationWebApi.GetItem(id);
                model.ResumeId = resumeId;

                return View(model);
            }, View("Offline"));
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemEdit(EducationItemModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            {
                await _educationWebApi.UpdateItem(model.Id, model);
                return RedirectToAction("ItemIndex", new { educationId = model.EducationId, resumeId = model.ResumeId });
            }, View("Offline"));

        }

        [HttpPost]
        public async Task<IActionResult> ItemDelete(int id)
        {
            return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            {
                await _educationWebApi.DeleteItem(id);
                return RedirectToAction("Index");
            }, View("Offline"));
        }

        #endregion
    }
}
