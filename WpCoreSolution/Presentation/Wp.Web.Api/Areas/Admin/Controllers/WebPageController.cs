using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wp.Core.Domain.WebPages;
using Wp.Services.Sections;
using Wp.Services.WebPages;
using Wp.Web.Api.Controllers;
using Wp.Web.Api.Extensions.Mapper;
using Wp.Web.Api.Models.Admin;

namespace Wp.Web.Api.Areas.Admin.Controllers
{
    [Produces("application/json")]
    [Route("api/admin/[controller]")]
    //[ApiController]
    [ValidateModel]
    public class WebPageController : ControllerBase
    {
        private readonly IWebPageService _webPageService;
        private readonly ISectionService _sectionService;

        public WebPageController(IWebPageService webPageService, ISectionService sectionService)
        {
            _webPageService = webPageService;
            _sectionService = sectionService;
        }

        #region Utitlites

        private void PrepareModels(WebPage entity, WebPageModel model)
        {
            var roles = _webPageService.GetRolesByPageId(entity.Id).ToList();
            var roleModelList = new List<WebPageModel.WebPageRoleModel>();
            roles.ForEach(x =>
            {
                roleModelList.Add(new WebPageModel.WebPageRoleModel { Id = x.Id, Name = x.Name });
            });
            model.Roles = roleModelList;
        }

        #endregion
        // GET: api/Page
        [HttpGet]  
        [Authorize]
        public ObjectResult Get()
        {
           var userClaims = User.Claims;
            var entities = _webPageService.GetAll();
            var models = entities.ToModels();
            return Ok(models);
        }

        // GET: api/Page/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var entity = _webPageService.GetById(id);
            var model = entity.ToModel();

            PrepareModels(entity, model);
            return Ok(model);
        }       

        // POST: api/Page
        [HttpPost]
        public IActionResult Post([FromBody]WebPageModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = model.ToEntity();
            entity.Id = 0;
            _webPageService.Insert(entity);
            return NoContent();
        }

        // PUT: api/Page/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]WebPageModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity =_webPageService.GetById(id);
            entity = model.ToEntity(entity);
            _webPageService.Update(entity);
            return NoContent();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
           var entity = _webPageService.GetById(id);
            _webPageService.Delete(entity);
        }

        #region import / export

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile()
        {
            string fileName = "demo.xlsx";
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            //currentDirectory = currentDirectory + "\\files";
            var file = Path.Combine(Path.Combine(currentDirectory, "files"), fileName);
           
            Stream stream = new FileStream(file, FileMode.Open, FileAccess.Read);

            //var memoryStream = new MemoryStream();

            //fileStream.Position = 0;
            //fileStream.CopyTo(memoryStream);

            if (stream == null)
                return NotFound(); // returns a NotFoundResult with Status404NotFound response.
            //application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
            //return File(stream, "application/octet-stream", fileName); // returns a FileStreamResult
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName); // returns a FileStreamResult
        }


        #endregion

        #region Sections

        public IActionResult GetAvailableSections()
        {
           var sections = _sectionService.GetAvailableSections();
            return Ok(sections);
        }
        
        [HttpGet("{pageId}/sections", Name = "Sections")]
        public IActionResult GetSections(int pageId)
        {
           var entities = _webPageService.GetSectionsByPageId(pageId).ToList();
            return Ok(entities);

        }

        #endregion

    }

}
