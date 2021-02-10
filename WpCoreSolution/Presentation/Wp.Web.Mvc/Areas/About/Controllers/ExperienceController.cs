using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nsr.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Core;
using Wp.Web.Mvc.About.Models;
using Wp.Web.Mvc.About.RestClients;
using Wp.Web.Mvc.Helpers;

namespace Wp.Web.Mvc.Areas.About.Controllers
{
    [Area("About")]
    public class ExperienceController : Controller
    {
        private readonly IExperienceWebApi _experienceWebApi;
        private readonly ILogger<ExperienceController> _logger;
        private readonly IWorkContext _workContext;
        private ResiliencyHelper _resiliencyHelper;

        public ExperienceController(IExperienceWebApi experienceWebApi, ILogger<ExperienceController> logger, IWorkContext workContext)
        {
            _experienceWebApi = experienceWebApi;
            _logger = logger;
            _workContext = workContext;
            _resiliencyHelper = new ResiliencyHelper(_logger);
        }
       
        #region Experience
        public async Task<IActionResult> Index(int resumeId)
        {
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {
                var model = await _experienceWebApi.GetExperiences(resumeId);

                return View(model);
            }, View("Offline"));
        }


        public async Task<IActionResult> Edit(int id)
        {
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {

                var model = await _experienceWebApi.GetExperienceById(id);

                return View(model);
            }, View("Offline"));
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExperienceModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            {
                await _experienceWebApi.Update(model.Id, model);
                return RedirectToAction("Index");
            }, View("Offline"));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            {
                await _experienceWebApi.Delete(id);
                return RedirectToAction("Index");
            }, View("Offline"));
        }

        #endregion

        #region Item

        public async Task<IActionResult> ProjectIndex(int experienceId, int resumeId)
        {
            ViewBag.ResumeId = resumeId;
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {
                var model = await _experienceWebApi.GetItems(experienceId);

                return View(model);
            }, View("Offline"));
        }


        public async Task<IActionResult> ProjectEdit(int id)
        {
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {

                var model = await _experienceWebApi.GetItem(id);

                return View(model);
            }, View("Offline"));
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ProjectEdit(ProjectModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            {
                await _experienceWebApi.UpdateItem(model.Id, model);
                return RedirectToAction("Index");
            }, View("Offline"));

        }

        [HttpPost]
        public async Task<IActionResult> ProjectDelete(int id)
        {
            return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            {
                await _experienceWebApi.DeleteItem(id);
                return RedirectToAction("Index");
            }, View("Offline"));
        }

        #endregion
    }
}
