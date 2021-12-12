using Microsoft.AspNetCore.Mvc;
using Nrs.RestClient;
using Nsr.Common.Services;
using Nsr.RestClient;
using Nsr.RestClient.Models.WorkHistories;
using Nsr.RestClient.RestClients.Localization;
using Nsr.Wh.Web.Domain;
using Nsr.Wh.Web.Services;

namespace Nsr.Wh.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EducationController : WpBaseController
    {
        private readonly ILocalizedEnitityHelperService _localizedEnitityHelperService;
        private readonly IEducationService _educationService;
        private readonly ILanguageWebApi _languageWebApi;

        public EducationController(ILocalizedEnitityHelperService localizedEnitityHelperService,
            IEducationService educationService,
            ILanguageWebApi languageWebApi)
        {
            _localizedEnitityHelperService = localizedEnitityHelperService;
            _educationService = educationService;
            _languageWebApi = languageWebApi;
        }

        #region Utilities

        [NonAction]
        protected void UpdateLocales(Education entity, EducationModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEnitityHelperService.SaveLocalizedValue(entity,
                                                            x => x.Name,
                                                            localized.Name,
                                                            localized.LanguageId);
            }
        }

        [NonAction]
        protected void UpdateLocales(EducationItem entity, EducationItemModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEnitityHelperService.SaveLocalizedValue(entity,
                                                            x => x.Name,
                                                            localized.Name,
                                                            localized.LanguageId);
                _localizedEnitityHelperService.SaveLocalizedValue(entity,
                                                            x => x.Place,
                                                            localized.Place,
                                                            localized.LanguageId);
                _localizedEnitityHelperService.SaveLocalizedValue(entity,
                                                           x => x.Period,
                                                           localized.Period,
                                                           localized.LanguageId);
                _localizedEnitityHelperService.SaveLocalizedValue(entity,
                                                           x => x.Description,
                                                           localized.Description,
                                                           localized.LanguageId);
            }
        }

        #endregion

        #region Education
        [HttpGet("GetByResumeId/{resumeId}")]
        public IActionResult GetByResumeId(int resumeId)
        {
            var entities = _educationService.GetAll(resumeId);
            var model = entities.ToModels();
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = _educationService.GetById(id);
            var model = entity.ToModel();

            //locals
            AddLocales(_languageWebApi, model.Locales, (locale, languageId) =>
            {
                locale.Name = entity.GetLocalized(x => x.Name, languageId, false, false);
            });

            return Ok(model);
        }

        [HttpPost]
        public IActionResult Post([FromBody] EducationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = model.ToEntity();
            entity.Id = 0;
            _educationService.Insert(entity);

            //locales
            UpdateLocales(entity, model);

            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] EducationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _educationService.GetById(model.Id);
            entity = model.ToEntity(entity);
            _educationService.Update(entity);

            //locales
            UpdateLocales(entity, model);

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var entity = _educationService.GetById(id);
            _educationService.Delete(entity);

            return NoContent();
        }

        #endregion

        #region Item
        [HttpGet("items/{educationId}")]
        public IActionResult GetItems(int educationId)
        {
            var entities = _educationService.GetEducationItemsByEducationId(educationId);
            var model = entities.ToModels();

            return Ok(model);
        }

        [HttpGet("item/{id}")]
        public IActionResult GetItem(int id)
        {
            var entity = _educationService.GetEducationItemById(id);
            var model = entity.ToModel();

            //locals
            AddLocales(_languageWebApi, model.Locales, (locale, languageId) =>
            {
                locale.Name = entity.GetLocalized(x => x.Name, languageId, false, false);
                locale.Place = entity.GetLocalized(x => x.Place, languageId, false, false);
                locale.Period = entity.GetLocalized(x => x.Period, languageId, false, false);
                locale.Description = entity.GetLocalized(x => x.Description, languageId, false, false);
            });

            return Ok(model);
        }

        [HttpPost("item")]
        public ActionResult PostItem([FromBody] EducationItemModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _educationService.GetEducationItemById(model.Id);
            entity = model.ToEntity(entity);
            entity.Id = 0;
            _educationService.UpdateEducationItem(entity);

            //locales
            UpdateLocales(entity, model);

            return NoContent();
        }

        [HttpPut("item/{id}")]
        public ActionResult PutItem(int id, [FromBody] EducationItemModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _educationService.GetEducationItemById(model.Id);
            entity = model.ToEntity(entity);
            _educationService.UpdateEducationItem(entity);

            //locales
            UpdateLocales(entity, model);

            return NoContent();
        }

        [HttpDelete("item/{id}")]
        public ActionResult DeleteItem(int id)
        {
            var entity = _educationService.GetEducationItemById(id);
            _educationService.DeleteEducationItem(entity);
            return NoContent();

        }
        #endregion
    }
}
