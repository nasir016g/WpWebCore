using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wp.Localization.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Wp.Localization.ManagementApi.Controllers
{
    [Route("api/[controller]")]
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
