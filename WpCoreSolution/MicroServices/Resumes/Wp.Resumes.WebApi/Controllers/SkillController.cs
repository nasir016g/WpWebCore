using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nsr.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Resumes.Core.Domain;
using Wp.Resumes.Services;
using Wp.Resumes.WebApi.Models;

namespace Wp.Resumes.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SkillController : WpBaseController
    {
         private readonly ILocalizedEntityService _localizedEntityService;
        private readonly ISkillService _skillService;
        private readonly ILanguageService _languageService;

        public SkillController(ILocalizedEntityService localizedEntityService,
            ISkillService skillService,
            ILanguageService languageService)
        {
            _localizedEntityService = localizedEntityService;
            _skillService = skillService;
            _languageService = languageService;
        }

        #region Utilities

        [NonAction]
        protected void UpdateLocales(Skill entity, SkillModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
            }
        }

        [NonAction]
        protected void UpdateLocales(SkillItem entity, SkillItemModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(entity,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
            }
        }

        #endregion

        #region Ski
        [HttpGet("GetByResumeId/{resumeId}")]
        public IActionResult GetByResumeId(int resumeId)
        {
            var entities = _skillService.GetAll(resumeId);
            var model = entities.ToModels();
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = _skillService.GetById(id);
            var model = entity.ToModel();

            //locals
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = entity.GetLocalized(x => x.Name, languageId, false, false);
            });

            return Ok(model);
        }

        [HttpPost]
        public IActionResult Post([FromBody] SkillModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = model.ToEntity();
            entity.Id = 0;
            _skillService.Insert(entity);

            //locales
            UpdateLocales(entity, model);

            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] SkillModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _skillService.GetById(model.Id);
            entity = model.ToEntity(entity);
            _skillService.Update(entity);

            //locales
            UpdateLocales(entity, model);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var entity = _skillService.GetById(id);
            _skillService.Delete(entity);

            return NoContent();
        }

        #endregion

        #region Item
        [HttpGet("items/{skillId}")]
        public IActionResult GetItems(int skillId)
        {
            var entities = _skillService.GetSkillItemsBySkillId(skillId);
            var model = entities.ToModels();

            return Ok(model);
        }

        [HttpGet("item/{id}")]
        public IActionResult GetItem(int id)
        {
            var entity = _skillService.GetSkillItemById(id);
            var model = entity.ToModel();

            //locals
            AddLocales(_languageService, model.Locales, (locale, languageId) =>
            {
                locale.Name = entity.GetLocalized(x => x.Name, languageId, false, false);
            });

            return Ok(model);
        }

        [HttpPost("item")]
        public ActionResult PostItem([FromBody] SkillItemModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _skillService.GetSkillItemById(model.Id);
            entity = model.ToEntity(entity);
            entity.Id = 0;
            _skillService.UpdateSkillItem(entity);

            //locales
            UpdateLocales(entity, model);

            return NoContent();
        }

        [HttpPut("item/{id}")]
        public ActionResult PutItem(int id, [FromBody] SkillItemModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _skillService.GetSkillItemById(model.Id);
            entity = model.ToEntity(entity);
            _skillService.UpdateSkillItem(entity);

            //locales
            UpdateLocales(entity, model);

            return NoContent();
        }

        [HttpDelete("item/{id}")]
        public ActionResult DeleteItem(int id)
        {
            var entity = _skillService.GetSkillItemById(id);
            _skillService.DeleteSkillItem(entity);
            return NoContent();

        }
        #endregion
    }
}
