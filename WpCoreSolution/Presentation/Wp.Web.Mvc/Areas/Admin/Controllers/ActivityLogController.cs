using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nsr.RestClient.Models.ActivityLogs;
using Nsr.RestClient.RestClients.ActivityLogs;
using System.Threading.Tasks;

namespace Wp.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActivityLogController : Controller
    {
        private readonly IActivityLogWebApi _activityLogWebApi;
        private readonly ILogger<ActivityLogController> _logger;

        public ActivityLogController(IActivityLogWebApi activityLogWebApi, ILogger<ActivityLogController> logger)
        {
            _activityLogWebApi = activityLogWebApi;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _activityLogWebApi.GetAll();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _activityLogWebApi.GetById(id);
            return View(model);
        }     


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {           
            await _activityLogWebApi.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
