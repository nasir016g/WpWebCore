
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nsr.Common.Core.Localization;
using Nsr.Common.Services;
using System;
using System.IO;
using System.Linq;
using Wp.Web.Mvc.Helpers;

namespace Wp.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LocalizationController : Controller
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly ILogger _logger;

        public LocalizationController(
            ILocalizationService localizationService,
            ILanguageService languageService,
            ILogger<LocalizationController> logger)
        {
            _localizationService = localizationService;
            _languageService = languageService;
            _logger = logger;
        }

        #region Language

        [HttpGet]
        public IActionResult Index()
        {
           var model = _languageService.GetAll();           

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {           
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
            var entity = _languageService.GetById(model.Id);
            entity.FlagImageFileName = model.FlagImageFileName;
            entity.DisplayOrder = model.DisplayOrder;
            entity.LanguageCulture = model.LanguageCulture;
            entity.Name = model.Name;
            entity.Published = model.Published;
            _languageService.Update(entity);

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
        
        #endregion

        #region Resources

        public IActionResult Resources(int languageId)
        {
            ViewBag.LanguageId = languageId;
            var model = _localizationService.GetAll().Where(x => x.LanguageId == languageId).ToList();
            return View(model);
        }        
       

        public IActionResult ResourceEdit(int id)
        {            
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

            _localizationService.Update(model);
            return RedirectToAction("Resources");
        }

        

        [HttpPost]
        public IActionResult ResourceDelete(int id)
        {            
           var entity = _localizationService.GetById(id);
            _localizationService.Delete(entity);

            return RedirectToAction("Resources");
        }

        #endregion

        #region Export / Import

        [HttpPost("ImportXml")]
        public IActionResult ImportXml(int languageId, IFormFile file)
        {
            var language = _languageService.GetById(languageId);
            if (language == null)
                //No language found with the specified id
                return BadRequest("Language is not found");

            if (file == null || file.Length == 0) return null;

            try
            {
                using (var sr = new StreamReader(file.OpenReadStream()))
                {
                    string content = sr.ReadToEnd();
                    _localizationService.ImportResourcesFromXml(language, content);
                }
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException(exc.Message);
            }

            return NoContent();
        }
        
        #endregion
    }
}

    