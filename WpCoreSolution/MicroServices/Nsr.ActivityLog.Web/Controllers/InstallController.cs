using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nsr.ActivityLogs.Web.Service.Installation;

namespace Nsr.ActivityLogs.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstallController : ControllerBase
    {
        private readonly IActivityLogInstallationService _installService;

        public InstallController(IActivityLogInstallationService installService)
        {
            _installService = installService;
        }

        [HttpGet]
        public IActionResult Data()
        {
            _installService.InstallData();
            return Ok("installed activity log types successfully.");
        }
    }
}
