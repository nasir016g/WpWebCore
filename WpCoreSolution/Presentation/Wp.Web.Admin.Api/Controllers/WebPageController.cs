using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wp.Core.Domain.Security;
using Wp.Core.Domain.WebPages;
using Wp.Services.Sections;
using Wp.Services.WebPages;
using Wp.Web.Framework.Extensions.Mapper;
using Wp.Web.Framework.Models.Admin;
using Wp.Web.Framework.Mvc.Filters;

namespace Wp.Web.Api.Admin.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    //[ValidateModel]
    public class WebPageController : ControllerBase
    {
        private readonly IWebPageService _webPageService;
        private readonly ISectionService _sectionService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public WebPageController(IWebPageService webPageService, ISectionService sectionService, RoleManager<IdentityRole> roleManager)
        {
            _webPageService = webPageService;
            _sectionService = sectionService;
            _roleManager = roleManager;
            
        }

        #region Utitlites

        private List<WebPageModel.WebPageRoleModel> GetAllRoles(WebPage webPage)
        {
            List<WebPageModel.WebPageRoleModel> allModels = (from role in _roleManager.Roles
                                                where !(role.Name == SystemRoleNames.Administrators.ToString() || role.Name == SystemRoleNames.Users.ToString())
                                                select new WebPageModel.WebPageRoleModel { Name = role.Name }).ToList();

            foreach (var model in allModels)
            {
                var role = webPage.Roles.FirstOrDefault(x => x.Name == model.Name);
                if (role != null)
                {
                    model.PermissionLevel = role.PermissionLevel;
                }
            }

            return allModels;
        }

        private void PrepareModels(WebPage entity, WebPageModel model)
        {
            //var roles = _webPageService.GetRolesByPageId(entity.Id).ToList();
            //var roleModelList = new List<WebPageModel.WebPageRoleModel>();
            //roles.ForEach(x =>
            //{
            //    roleModelList.Add(new WebPageModel.WebPageRoleModel { Id = x.Id, Name = x.Name });
            //});
            //model.Roles = roleModelList;
            model.Roles = GetAllRoles(entity);
        }

        #endregion
        // GET: api/Page
        [HttpGet]  
        //[Authorize]
        public IActionResult Get()
        {
           //var userClaims = User.Claims;
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
