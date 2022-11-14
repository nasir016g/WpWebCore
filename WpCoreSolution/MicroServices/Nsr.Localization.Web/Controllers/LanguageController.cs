using Microsoft.AspNetCore.Mvc;
using Nsr.Localization.Web.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nsr.Localization.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        // GET: api/<LanguageController>
        [HttpGet]
        public IActionResult Get()
        {
            var entities = _languageService.GetAll().ToList();
            //var model = entities.ToModels();
            return Ok(entities);
        }

        // GET api/<LanguageController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = _languageService.GetById(id);
            return Ok(entity);
        }

        // POST api/<LanguageController>
        [HttpPost]
        public IActionResult Post([FromBody] Language model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var entity = model.ToEntity();
            var entity = model;
            entity.Id = 0;
            _languageService.Insert(entity);

            return NoContent();
        }

        // PUT api/<LanguageController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Language model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = model;//model.ToEntity();
            _languageService.Update(entity);

            return NoContent();
        }

        // DELETE api/<LanguageController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _languageService.GetById(id);
            _languageService.Delete(entity);
            return RedirectToAction("Get");
        }
    }
}
