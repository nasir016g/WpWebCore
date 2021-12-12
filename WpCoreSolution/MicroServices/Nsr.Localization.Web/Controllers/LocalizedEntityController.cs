using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nsr.Localization.Web.Services;
using Nsr.RestClient.Models.Localization;

namespace Nsr.Localization.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizedEntityController : ControllerBase
    {
        private readonly ILocalizedEntityService _localizedEntityService;

        public LocalizedEntityController(ILocalizedEntityService localizedEntityService)
        {
            _localizedEntityService = localizedEntityService;
        }

        [HttpGet("GetLocalizedValue/{languageId}/{entityId}/{localeKeyGroup}/{localeKey}")]
        public IActionResult GetLocalizedProperties(int languageId, int entityId, string localeKeyGroup, string localeKey)
        {
            var value = _localizedEntityService.GetLocalizedValue(languageId, entityId, localeKeyGroup, localeKey);
            //var model = entities.ToModels();
            return Ok(value);
        }


        [HttpGet("GetLocalizedProperties/{entityId}/{localeKeyGroup}")]
        public IActionResult GetLocalizedProperties(int entityId, string localeKeyGroup)
        {
            var entities = _localizedEntityService.GetLocalizedProperties(entityId, localeKeyGroup);
            //var model = entities.ToModels();
            return Ok(entities);
        }


        [HttpPost]
        public IActionResult InsertLocalizedProperty([FromBody] LocalizedProperty model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var entity = model.ToEntity();
            var entity = model;
            entity.Id = 0;
            _localizedEntityService.InsertLocalizedProperty(entity);

            return NoContent();
        }

        // PUT api/<LanguageController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateLocalizedProperty(int id, [FromBody] LocalizedProperty model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = model;//model.ToEntity();
            _localizedEntityService.UpdateLocalizedProperty(entity);

            return NoContent();
        }

        // DELETE api/<LanguageController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _localizedEntityService.GetLocalizedPropertyById(id);
            _localizedEntityService.DeleteLocalizedProperty(entity);
            return NoContent();
        }
    }
}
