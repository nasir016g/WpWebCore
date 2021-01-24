using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wp.Localization.Core;
using Wp.Localization.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Wp.Localization.ManagementApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LocalizationController : ControllerBase
    {
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;

        public LocalizationController(ILanguageService languageService, ILocalizationService localizationService)
        {
            _languageService = languageService;
            _localizationService = localizationService;
        }

        [HttpGet("language/{languageId}")]
        public IActionResult Get(int languageId)
        {
            var entities = _localizationService.GetAll().Where(x => x.LanguageId == languageId);
            //var model = entities.ToModels();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var entity = _localizationService.GetById(id);
            return Ok(entity);
        }

        [HttpPost]
        public IActionResult Post([FromBody] LocaleStringResource entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            entity.Id = 0;
            _localizationService.Insert(entity);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] LocaleStringResource model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = _localizationService.GetById(id);
            entity.LanguageId = model.LanguageId;
            entity.ResourceName = model.ResourceName;
            entity.ResourceValue = model.ResourceValue;

            _localizationService.Update(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var entity = _localizationService.GetById(id);
            _localizationService.Delete(entity);
        }

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
    }
}
