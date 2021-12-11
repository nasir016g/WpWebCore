using Microsoft.AspNetCore.Mvc;
using Nsr.Localization.Web.Api.Services;
using Nsr.RestClient.Models.Localization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nsr.Localization.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizationController : ControllerBase
    {
        private readonly ILocalizationService _localizationService;

        public LocalizationController(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var model = _localizationService.GetAll();
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var model = _localizationService.GetById(id);
            return Ok(model);
        }


        [HttpGet("GetAllResourceValues/{languageId}")]
        public IActionResult GetAllResourceValues(int languageId)
        {
            var model = _localizationService.GetAllResourceValues(languageId).ToList();
           return Ok(model);
        }

        [HttpGet("GetResource/{resourceKey}")]
        public IActionResult GetResource(string resourceKey)
        {
            var model = _localizationService.GetResource(resourceKey);
            return Ok(model);
        }

        [HttpGet("GetResource/{resourceKey}/{languageId}/{defaultValue?}")]
        public IActionResult GetResource(string resourceKey, int languageId, string defaultValue = "")
        {
            var model = _localizationService.GetResource(resourceKey, languageId, defaultValue);
            return Ok(model);
        }        

        [HttpPost]
        public IActionResult Post(LocaleStringResource model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = model;
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
            var entity = model;//model.ToEntity();
            _localizationService.Update(entity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _localizationService.GetById(id);
             _localizationService.Delete(entity);

            return RedirectToAction("Get");
        }

        


    }
}
