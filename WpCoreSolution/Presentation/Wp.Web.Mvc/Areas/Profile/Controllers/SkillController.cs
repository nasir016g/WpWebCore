﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nsr.Common.Core;
using System.Threading.Tasks;
using Wp.Web.Mvc.Profile.RestClients;
using Wp.Web.Mvc.Helpers;
using Nsr.Common.Core.Models;

namespace Wp.Web.Mvc.Areas.Profile.Controllers
{
    [Area("Profile")]
    public class SkillController : Controller
    {
        private readonly ISkillWebApi _skillWebApi;
        private readonly ILogger<SkillController> _logger;
        private readonly IWorkContext _workContext;
        private ResiliencyHelper _resiliencyHelper;

        public SkillController(ISkillWebApi skillWebApi, ILogger<SkillController> logger, IWorkContext workContext)
        {
            _skillWebApi = skillWebApi;
            _logger = logger;
            _workContext = workContext;
            _resiliencyHelper = new ResiliencyHelper(_logger);
        }
        #region Skill
        public async Task<IActionResult> Index(int resumeId)
        {
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {
                var model = await _skillWebApi.GetSkills(resumeId);

                return View(model);
            }, View("Offline"));
        }


        public async Task<IActionResult> Edit(int id)
        {
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {

                var model = await _skillWebApi.GetSkillById(id);

                return View(model);
            }, View("Offline"));
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SkillModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            {
                await _skillWebApi.Update(model.Id, model);
                return RedirectToAction("Index");
            }, View("Offline"));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            {
                await _skillWebApi.Delete(id);
                return RedirectToAction("Index");
            }, View("Offline"));
        }

        #endregion

        #region Item

        public async Task<IActionResult> ItemIndex(int skillId, int resumeId)
        {
            ViewBag.ResumeId = resumeId;
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {
                var model = await _skillWebApi.GetItems(skillId);

                return View(model);
            }, View("Offline"));
        }


        public async Task<IActionResult> ItemEdit(int id)
        {
            return await _resiliencyHelper.ExecuteResilient(async () =>
            {

                var model = await _skillWebApi.GetItem(id);

                return View(model);
            }, View("Offline"));
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ItemEdit(SkillItemModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            {
                await _skillWebApi.UpdateItem(model.Id, model);
                return RedirectToAction("Index");
            }, View("Offline"));

        }

        [HttpPost]
        public async Task<IActionResult> ItemDelete(int id)
        {
            return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            {
                await _skillWebApi.DeleteItem(id);
                return RedirectToAction("Index");
            }, View("Offline"));
        }

        #endregion
    }
}
