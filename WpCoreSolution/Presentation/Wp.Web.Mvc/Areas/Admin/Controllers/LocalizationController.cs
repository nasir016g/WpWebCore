
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nsr.RestClient.Models.Localization;
using Nsr.RestClient.RestClients.Localization;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Wp.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LocalizationController : Controller
    {
        private readonly ILocalizationWebApi _localizationWebApi;
        private readonly ILanguageWebApi _languageWebApi;
        private readonly ILogger _logger;

        public LocalizationController(
            ILocalizationWebApi localizationWebApi,
            ILanguageWebApi languageWebApi,
            ILogger<LocalizationController> logger)
        {
            _localizationWebApi = localizationWebApi;
            _languageWebApi = languageWebApi;
            _logger = logger;
        }

        #region Language

        [HttpGet]
        public IActionResult Index()
        {
           var model = _languageWebApi.GetAll();           

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {           
           var model = _languageWebApi.GetById(id);
           return View(model);
        }

        public IActionResult Edit(int id)
        {
            //return await _resiliencyHelper.ExecuteResilient(async () =>
            //{

            //    var model = await _localizationManagementApi.GetLanguageById(id);

            //    return View(model);
            //}, View("Offline"));
            var model = _languageWebApi.GetById(id);
            return View(model);
        }

      
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Language model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var entity = await _languageWebApi.GetById(model.Id);
            entity.FlagImageFileName = model.FlagImageFileName;
            entity.DisplayOrder = model.DisplayOrder;
            entity.LanguageCulture = model.LanguageCulture;
            entity.Name = model.Name;
            entity.Published = model.Published;
            await _languageWebApi.Update(model.Id, entity);

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            //return await _resiliencyHelper.ExecuteResilient<IActionResult>(async () =>
            //{
            //    await _localizationManagementApi.Delete(id);
            //    return RedirectToAction("Index");
            //}, View("Offline"));
          // var entity = await _languageWebApi.GetById(id);
           await _languageWebApi.Delete(id);
            return RedirectToAction("Index");
        }
        
        #endregion

        #region Resources

        public IActionResult Resources(int languageId)
        {
            ViewBag.LanguageId = languageId;
            var model = _localizationWebApi.GetAll().GetAwaiter().GetResult().Where(x => x.LanguageId == languageId).ToList();
            return View(model);
        }        
       

        public async Task<IActionResult> ResourceEdit(int id)
        {            
            var model = await _localizationWebApi.GetById(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResourceEdit(LocaleStringResource model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }           

            await _localizationWebApi.Update(model.Id, model);
            return RedirectToAction("Resources");
        }

        

        [HttpPost]
        public async Task<IActionResult> ResourceDelete(int id)
        {            
           //var entity = _localizationWebApi.GetById(id);
            await _localizationWebApi.Delete(id);

            return RedirectToAction("Resources");
        }

        #endregion

        #region Export / Import

        [HttpPost("ImportXml")]
        public async Task<IActionResult> ImportXml(int languageId, IFormFile file)
        {
            var language = _languageWebApi.GetById(languageId);
            if (language == null)
                //No language found with the specified id
                return BadRequest("Language is not found");

            if (file == null || file.Length == 0) return null;

            try
            {
                using (var sr = new StreamReader(file.OpenReadStream()))
                {
                    string content = sr.ReadToEnd();
                  // await _localizationWebApi.ImportResourcesFromXml(language, content); // not implemented yet
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

    