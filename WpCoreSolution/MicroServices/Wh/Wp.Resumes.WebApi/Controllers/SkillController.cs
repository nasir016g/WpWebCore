﻿using Microsoft.AspNetCore.Mvc;
using Nrs.RestClient;
using Nsr.Common.Services;
using Nsr.RestClient;
using Nsr.RestClient.Models.WorkHistories;
using Nsr.RestClient.RestClients.Localization;
using Wp.Wh.Core.Domain;
using Wp.Wh.Services;

namespace Wp.Wh.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SkillController : WpBaseController
    {
         private readonly ILocalizedEnitityHelperService _localizedEnitityHelperService;
        private readonly ISkillService _skillService;
        private readonly ILanguageWebApi _languageWebApi;

        public SkillController(ILocalizedEnitityHelperService localizedEnitityHelperService,
            ISkillService skillService,
            ILanguageWebApi languageWebApi)
        {
            _localizedEnitityHelperService = localizedEnitityHelperService;
            _skillService = skillService;
            _languageWebApi = languageWebApi;
        }

        #region Utilities

        [NonAction]
        protected void UpdateLocales(Skill entity, SkillModel model)
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
        protected void UpdateLocales(SkillItem entity, SkillItemModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEnitityHelperService.SaveLocalizedValue(entity,
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
            AddLocales(_languageWebApi, model.Locales, (locale, languageId) =>
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
            AddLocales(_languageWebApi, model.Locales, (locale, languageId) =>
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
