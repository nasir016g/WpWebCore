using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nsr.Localization.Web.Service;

namespace Nsr.Localization.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstallController : ControllerBase
    {
        private readonly ILocalizationInstallationService _installService;

        public InstallController(ILocalizationInstallationService installService)
        {
            _installService = installService;
        }
        [HttpGet]
        public IActionResult Data()
        {
             _installService.InstallData();
            return Ok("installed localization data successfully.");
        }
    }
}
