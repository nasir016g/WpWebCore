using Microsoft.AspNetCore.Mvc;
using Nsr.Common.Service.Localization;
using Nsr.Common.Services;
using Nsr.RestClient.Models.WorkHistories;
using Nsr.Work.Web.Domain;
using Nsr.Work.Web.Services;

namespace Nsr.Work.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExperienceController : WpBaseController
    {
        private readonly ILocalizedEntityService _localizedEnitityService;
        private readonly IExperienceService _experienceService;
        private readonly ILanguageService _languageService;

        public ExperienceController(ILocalizedEntityService localizedEnitityService,
            IExperienceService experienceService,
            ILanguageService languageService)
        {
            _localizedEnitityService = localizedEnitityService;
            _experienceService = experienceService;
            _languageService = languageService;
        }

        #region Utilities

        [NonAction]
        protected void UpdateLocales(Experience entity, ExperienceModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEnitityService.SaveLocalizedValue(entity,
                                                        x => x.Name,
                                                        localized.Name,
                                                        localized.LanguageId);
                _localizedEnitityService.SaveLocalizedValue(entity,
                                                        x => x.Period,
                                                        localized.Period,
                                                        localized.LanguageId);
                _localizedEnitityService.SaveLocalizedValue(entity,
                                                       x => x.Function,
                                                       localized.Function,
                                                       localized.LanguageId);
                _localizedEnitityService.SaveLocalizedValue(entity,
                                                       x => x.Tasks,
                                                       localized.Tasks,
                                                       localized.LanguageId);
            }
        }


        [NonAction]
        protected void UpdateLocales(Project entity, ProjectModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEnitityService.SaveLocalizedValue(entity,
                                                        x => x.Name,
                                                        localized.Name,
                                                        localized.LanguageId);
                _localizedEnitityService.SaveLocalizedValue(entity,
                                                       x => x.Description,
                                                       localized.Description,
                                                       localized.LanguageId);
                _localizedEnitityService.SaveLocalizedValue(entity,
                                                       x => x.Technology,
                                                       localized.Technology,
                                                       localized.LanguageId);
            }
        }
        #endregion

        #region We
        [HttpGet("GetByResumeId/{resumeId}")]
        public IActionResult GetByResumeId(int resumeId)
        {
            var entities = _experienceService.GetAll(resumeId);
            var model = entities.ToModels();
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = _experienceService.GetById(id);
            var model = entity.ToModel();

            //locals
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = entity.GetLocalized(x => x.Name, languageId, false, false);
                locale.Period = entity.GetLocalized(x => x.Period, languageId, false, false);
                locale.Function = entity.GetLocalized(x => x.Function, languageId, false, false);
                locale.Tasks = entity.GetLocalized(x => x.Tasks, languageId, false, false);
            });

            return Ok(model);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ExperienceModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = model.ToEntity();
            entity.Id = 0;
            _experienceService.Insert(entity);

            //locales
            UpdateLocales(entity, model);

            return NoContent();
        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ExperienceModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _experienceService.GetById(model.Id);
            entity = model.ToEntity(entity);
            _experienceService.Update(entity);

            //locales
            UpdateLocales(entity, model);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var entity = _experienceService.GetById(id);
            _experienceService.Delete(entity);

            return NoContent();
        }

        #endregion

        #region Project
        [HttpGet("items/{experienceId}")]
        public IActionResult GetItems(int experienceId)
        {
            var entities = _experienceService.GetProjectsByExperienceId(experienceId);
            var model = entities.ToModels();

            return Ok(model);
        }
        [HttpGet("item/{id}")]
        public IActionResult GetItem(int id)
        {
            var entity = _experienceService.GetProjectById(id);
            var model = entity.ToModel();

            //locals
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = entity.GetLocalized(x => x.Name, languageId, false, false);
                locale.Description = entity.GetLocalized(x => x.Description, languageId, false, false);
                locale.Technology = entity.GetLocalized(x => x.Technology, languageId, false, false);
            });

            return Ok(model);
        }
        [HttpPost("item")]
        public ActionResult PostItem([FromBody] ProjectModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _experienceService.GetProjectById(model.Id);
            entity = model.ToEntity(entity);
            entity.Id = 0;
            _experienceService.UpdateProject(entity);

            //locales
            UpdateLocales(entity, model);

            return NoContent();
        }
        [HttpPut("item/{id}")]
        public ActionResult PutItem(int id, [FromBody] ProjectModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _experienceService.GetProjectById(model.Id);
            entity = model.ToEntity(entity);
            _experienceService.UpdateProject(entity);

            //locales
            UpdateLocales(entity, model);

            return NoContent();
        }

        [HttpDelete("item/{id}")]
        public ActionResult DeleteItem(int id)
        {
            var entity = _experienceService.GetProjectById(id);
            _experienceService.DeleteProject(entity);
            return NoContent();

        }
        #endregion
    }
}
