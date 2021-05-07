using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nsr.Common.Core.Localization;
using Nsr.Common.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nsr.Common.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizationController : ControllerBase
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;

        public LocalizationController(ILocalizationService localizationService, ILanguageService languageService)
        {
            _localizationService = localizationService;
            _languageService = languageService;
        }

        #region Language

        [HttpGet]
        public IActionResult Get()
        {
            var model = _languageService.GetAll();

            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var model = _languageService.GetById(id);
            return Ok(model);
        }      


        [HttpPost]
        public IActionResult Post([FromBody] Language model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = model;
            //var entity = _languageService.GetById(model.Id);
            //entity.FlagImageFileName = model.FlagImageFileName;
            //entity.DisplayOrder = model.DisplayOrder;
            //entity.LanguageCulture = model.LanguageCulture;
            //entity.Name = model.Name;
            //entity.Published = model.Published;
            _languageService.Insert(entity);

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Language model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = _languageService.GetById(model.Id);
            entity.FlagImageFileName = model.FlagImageFileName;
            entity.DisplayOrder = model.DisplayOrder;
            entity.LanguageCulture = model.LanguageCulture;
            entity.Name = model.Name;
            entity.Published = model.Published;
            _languageService.Update(entity);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _languageService.GetById(id);
            _languageService.Delete(entity);
            return NoContent();
        }

        #endregion

        #region Resources

        [HttpGet("resources/{languageId}")]
        public IActionResult GetResources(int languageId)
        {
            var model = _localizationService.GetAll().Where(x => x.LanguageId == languageId).ToList();
            return Ok(model);
        }


        [HttpPost("Resources/{id}")]
        public IActionResult GetResource(int id)
        {
            var model = _localizationService.GetById(id);
            return Ok(model);
        }

        [HttpPost("resource")]
        public IActionResult PostResource([FromBody]LocaleStringResource model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _localizationService.Insert(model);
            return NoContent();
        }

        [HttpPut("resource/{id}")]
        public IActionResult PutResource([FromBody] LocaleStringResource model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           var entity = _localizationService.GetById(model.Id);
            entity.ResourceName = model.ResourceName;
            entity.ResourceValue = model.ResourceValue;
            _localizationService.Update(model);

            return NoContent();
        }

        [HttpDelete("resource/{id}")]
        public IActionResult ResourceDelete(int id)
        {
            var entity = _localizationService.GetById(id);
            _localizationService.Delete(entity);

            return NoContent();
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
