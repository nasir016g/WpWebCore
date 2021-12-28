using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nsr.ActivityLogs.Web.Service.Abstract;
using Nsr.RestClient.Models.ActivityLogs;

namespace Nsr.ActivityLogs.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityLogController : ControllerBase
    {
        private readonly IActivityLogService _activityLogService;

        public ActivityLogController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var entities = _activityLogService.GetAll().ToList();
            //var model = entities.ToModels();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = _activityLogService.GetById(id);
            return Ok(entity);
        }

        [HttpPost("{systemKeyword}/{entityType}/{entityId}")]
        public IActionResult Post(string systemKeyword, string entityType, int entityId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var entity = model.ToEntity();
            _activityLogService.InsertActivity(systemKeyword, entityType, entityId);

            return NoContent();
        }

        [HttpPost("AuditEntries")]
        public IActionResult Post([FromBody] List<AuditEntry> auditEntries)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var entity = model.ToEntity();
            _activityLogService.InsertActivity(auditEntries);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _activityLogService.GetById(id);
            _activityLogService.Delete(entity);
            return RedirectToAction("Get");
        }
    }
}
