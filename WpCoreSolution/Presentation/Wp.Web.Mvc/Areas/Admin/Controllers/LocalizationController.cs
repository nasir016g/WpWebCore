using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Localization.Core;
using Wp.Localization.Services;
using Wp.Web.Mvc.Models;
using Wp.Web.Mvc.RestClients;

namespace Wp.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LocalizationController : Controller
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly ILogger _logger;
        private ResiliencyHelper _resiliencyHelper;

        public LocalizationController(
            ILocalizationService localizationService,
            ILanguageService languageService,
            ILogger<LocalizationController> logger)
        {
            _localizationService = localizationService;
            _languageService = languageService;
            _logger = logger;
            _resiliencyHelper = new ResiliencyHelper(_logger);
        }

        [HttpGet]
        public IActionResult Index()
        {
           var model = _languageService.GetAll();

            //return await _resiliencyHelper.ExecuteResilient(async () =>
            //{
            //    var model = await _localizationManagementApi.GetLanguages();

            //    return View(model);
            //}, View("Offline"));

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            //return await _resiliencyHelper.ExecuteResilient(async () =>
            //{

            //    var model = await _localizationManagementApi.GetLanguageById(id);

            //    return View(model);
            //}, View("Offline"));
           var model = _languageService.GetById(id);
           return View(model);
        }

        public IActionResult Edit(int id)
        {
            //return await _resiliencyHelper.ExecuteResilient(async () =>
            //{

            //    var model = await _localizationManagementApi.GetLanguageById(id);

            //    return View(model);
            //}, View("Offline"));
            var model = _languageService.GetById(id);
            return View(model);
        }

      
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit(Language model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _languageService.Update(model);

            //return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            //{
            //    await _localizationManagementApi.Update(model.Id, model);
            //    return RedirectToAction("Index");
            //}, View("Offline"));

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            //return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            //{
            //    await _localizationManagementApi.Delete(id);
            //    return RedirectToAction("Index");
            //}, View("Offline"));
           var entity = _languageService.GetById(id);
            _languageService.Delete(entity);
            return RedirectToAction("Index");
        }

        #region Resources

        public IActionResult Resources(int languageId)
        {
            //return await _resiliencyHelper.ExecuteResilient(async () =>
            //{
            //    var model = await _localizationManagementApi.GetResources(languageId);

            //    return View(model);
            //}, View("Offline"));

            var model = _localizationService.GetAll().Where(x => x.LanguageId == languageId).ToList();
            return View(model);
        }        
       

        public IActionResult ResourceEdit(int id)
        {
            //return await _resiliencyHelper.ExecuteResilient(async () =>
            //{

            //    var model = await _localizationManagementApi.GetResourceById(id);

            //    return View(model);
            //}, View("Offline"));
            var model = _localizationService.GetById(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult ResourceEdit(LocaleStringResource model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            //{
            //    await _localizationManagementApi.UpdateResource(model.Id, model);
            //    return RedirectToAction("Resources");
            //}, View("Offline"));

            _localizationService.Update(model);
            return RedirectToAction("Resources");
        }

        

        [HttpPost]
        public IActionResult ResourceDelete(int id)
        {
            //return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            //{
            //    await _localizationManagementApi.Delete(id);
            //    return RedirectToAction("Resources");
            //}, View("Offline"));

           var entity = _localizationService.GetById(id);
            _localizationService.Delete(entity);

            return RedirectToAction("Resources");
        }

        #endregion
    }
}

    