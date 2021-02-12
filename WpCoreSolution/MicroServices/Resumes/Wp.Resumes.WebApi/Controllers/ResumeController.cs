using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nsr.Common.Services;
using System;
using System.IO;
using System.Text;
using Wp.Resumes.Core.Domain;
using Wp.Resumes.Services;
using Wp.Resumes.Services.ExportImport;
using Wp.Resumes.WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Wp.Resumes.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ResumeController : WpBaseController
    {
        private readonly IResumeService _resumeService;
        private readonly IImportManager _importManager;
        private readonly IExportManager _exportManager;
        private readonly ILanguageService _languageService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPdfService _pdfService;

        public ResumeController(
            IResumeService resumeService,
            IImportManager importManager,
            IExportManager exportManager,
            ILanguageService languageService,
            ILocalizedEntityService localizedEntityService,
            IPdfService pdfService          


            )
        {
            _resumeService = resumeService;
            _importManager = importManager;
            _exportManager = exportManager;
            _languageService = languageService;
            _localizedEntityService = localizedEntityService;
            _pdfService = pdfService;
        }
        #region Properties


        //public IIdentityService _identityService { get; set; }
       // public UserManager<ApplicationUser> UserManager { get { return _identityService.UserManager; } }

        #endregion

        #region Utilities

        [NonAction]
        protected void PrepareResumeModel(ResumeModel model)
        {
        }

        [NonAction]
        protected void UpdateLocales(Resume entity, ResumeModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Summary,
                    localized.Summary,
                    localized.LanguageId);
            }
        }

        #endregion

        #region Resume
        [HttpGet]
        public IActionResult Get()
        {
            var entities = _resumeService.GetAll();
            var model = entities.ToModels();
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = _resumeService.GetById(id);
            var model = entity.ToModel();

            //locals
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Summary = entity.GetLocalized(x => x.Summary, languageId, false, false);
            });

            return Ok(model);
        }


       [HttpGet("details/{id}")]
        public IActionResult GetResumeDetails(int id)
        {
            //Including eductions, experiences and etc.
            var entity = _resumeService.GetDetails(id);
            return Ok(entity);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ResumeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = model.ToEntity();
            entity.Id = 0;
            _resumeService.Insert(entity);

            //locales
            UpdateLocales(entity, model);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ResumeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = model.ToEntity();
            _resumeService.Update(entity);

            //locales
            UpdateLocales(entity, model);
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var entity = _resumeService.GetById(id);
            _resumeService.Delete(entity);
            return RedirectToAction("Get");
        }

        #endregion      

        #region Import/Export
       
        [HttpPost("ImportXml")]
        public IActionResult ImportXml(string userName, IFormFile file)
        {

            if (file == null || file.Length == 0) return null;

            try
            {
                using (var sr = new StreamReader(file.OpenReadStream()))
                {
                    string content = sr.ReadToEnd();
                    //var currentUser = "test@test.nl";

                    var resume = _importManager.ImportWorkFromXml(content, userName);
                }
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException(exc.Message);
            }

            return NoContent();
        }

        [HttpGet("ExportToXml/{id}")]
        public ActionResult ExportToXml(int id)
        {
            try
            {
                var entity = _resumeService.GetById(id);
                var xml = _exportManager.ExportResumeToXml(entity);
                return File(Encoding.UTF8.GetBytes(xml), "application/xml", entity.Name + ".xml");
            }
            catch (Exception exc)
            {
                //ErrorNotification(exc, true);
                ModelState.AddModelError("ExportXml", exc.ToString());

                return RedirectToAction("Edit", new { id = id });
            }
        }


        [HttpGet("ExportToWord/{id}/{languageId}")]
        public ActionResult ExportToWord(int id, int languageId)
        {
            try
            {
                var entity = _resumeService.GetById(id);

                byte[] bytes = null;
                using (var stream = new MemoryStream())
                {
                    _exportManager.ExportResumeToWord(stream, entity, languageId);
                    bytes = stream.ToArray();
                }
                return File(bytes, "application/docx", string.Format("{0}_Resume.docx", entity.Name));
            }
            catch (Exception exc)
            {
                //ErrorNotification(exc);
                return RedirectToAction("Index");
            }
        }

        [HttpGet("PrintToPdf/{id}")]
        public ActionResult PrintToPdf(int id)
        {
            try
            {
                var entity = _resumeService.GetById(id);

                byte[] bytes = null;
                using (var stream = new MemoryStream())
                {
                    _pdfService.PrintResume(stream, entity);
                    bytes = stream.ToArray();
                }
                return File(bytes, "application/pdf", string.Format("{0}_Resume.pdf", entity.Name));
            }
            catch (Exception exc)
            {
                //ErrorNotification(exc);
                return RedirectToAction("Index");
            }
        }

        #endregion
    }
}
