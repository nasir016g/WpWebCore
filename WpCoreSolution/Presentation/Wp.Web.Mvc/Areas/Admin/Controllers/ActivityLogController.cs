using Microsoft.AspNetCore.Mvc;
using Nsr.RestClient.RestClients.ActivityLogs;
using System.Threading.Tasks;

namespace Wp.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ActivityLogController : Controller
    {
        private readonly IActivityLogWebApi _activityLogWebApi;

        public ActivityLogController(IActivityLogWebApi activityLogWebApi)
        {
            _activityLogWebApi = activityLogWebApi;
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
            
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _activityLogWebApi.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
