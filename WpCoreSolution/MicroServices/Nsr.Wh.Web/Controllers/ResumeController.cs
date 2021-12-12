using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nsr.Common.Services;
using Nsr.RestClient.Models.WorkHistories;
using Nsr.RestClient.RestClients.Localization;
using System;
using System.IO;
using System.Text;
using Nsr.Wh.Web.Domain;
using Nsr.Wh.Web.Services;
using Nsr.Wh.Web.Services.ExportImport;
using Nrs.RestClient;
using System.Threading.Tasks;
using Nsr.RestClient;
using Nsr.Common.Core;
using System.Text.RegularExpressions;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nsr.Wh.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ResumeController : WpBaseController
    {
        private readonly IWorkContext _workContext;
        private readonly IResumeService _resumeService;
        private readonly IImportManager _importManager;
        private readonly IExportManager _exportManager;
        private readonly ILanguageWebApi _languageWebApi;
        private readonly ILocalizedEnitityHelperService _localizedEnitityHelperService;
        private readonly IPdfService _pdfService;
        private ResiliencyHelper _resiliencyHelper;

        public ResumeController(
            IWorkContext workContext,
            IResumeService resumeService,
            IImportManager importManager,
            IExportManager exportManager,
            ILanguageWebApi languageWebApi,
            ILocalizedEnitityHelperService localizedEnitityHelperService,
            ILogger<ResumeController> logger,
            IPdfService pdfService
            )
        {
            _workContext = workContext;
            _resumeService = resumeService;
            _importManager = importManager;
            _exportManager = exportManager;
            _languageWebApi = languageWebApi;
            _localizedEnitityHelperService = localizedEnitityHelperService;
            _pdfService = pdfService;
            _resiliencyHelper = new ResiliencyHelper(logger);
        }
        #region Properties


        //public IIdentityService _identityService { get; set; }
        // public UserManager<ApplicationUser> UserManager { get { return _identityService.UserManager; } }

        #endregion

        #region Utilities

        [NonAction]
        protected WorkHistoryDetailsModel PrepareWorkHistoryDetailsModel(Resume workHistory)
        {
            var model = new WorkHistoryDetailsModel()
            {
                Id = workHistory.Id,

                Name = workHistory.Name,
                Address = workHistory.Address,
                PostalCode = workHistory.PostalCode,
                Town = workHistory.Town,
                Phone = workHistory.Phone,
                Mobile = workHistory.Mobile,
                Email = workHistory.Email,
                DateOfBirth = workHistory.DateOfBirth,
                Website = workHistory.Website
            };
            var summary = workHistory.GetLocalized(x => x.Summary);

            if (!String.IsNullOrWhiteSpace(summary))
            {
                model.Summary = summary.Replace("\r\n", "<br />");
                model.Summary = model.Summary.Replace("\n", "<br />");
            }

            #region Education

            foreach (var education in workHistory.Educations)
            {
                var educationModel = new EducationDetailsModels()
                {
                    Id = education.Id,
                    Name = education.GetLocalized(x => x.Name)
                };

                foreach (var educationItem in education.EducationItems)
                {
                    var educationItemModel = new EducationDetialItemModel()
                    {
                        Id = educationItem.Id,
                        Name = educationItem.GetLocalized(x => x.Name),
                        //Descriptions = Regex.Split(educationItem.GetLocalized(x => x.Description), "\r\n").ToList(),
                        Period = educationItem.GetLocalized(x => x.Period),
                        Place = educationItem.GetLocalized(x => x.Place)
                    };
                    var description = HtmlHelper.StripTags(educationItem.GetLocalized(x => x.Description));
                    description = description.Replace("-", "");

                    if (description != null && description.Contains("\r\n"))
                    {
                        educationItemModel.Descriptions = Regex.Split(description, "\r\n").ToList();
                        educationItemModel.Descriptions.RemoveAll(x => String.IsNullOrWhiteSpace(x));
                    }
                    else
                    {
                        educationItemModel.Descriptions.Add(description);
                    }

                    educationModel.EducationItems.Add(educationItemModel);
                }
                model.Educations.Add(educationModel);
            }

            #endregion

            #region Skill
            var skills = workHistory.Skills;
            foreach (var skill in skills.OrderBy(x => x.DisplayOrder))
            {
                var skillModel = new SkillDetailsModels()
                {
                    Id = skill.Id,
                    Name = skill.GetLocalized(x => x.Name)
                };

                foreach (var skillItem in skill.SkillItems.OrderByDescending(x => x.Level))
                {
                    var skillItemModel = new SkillItemDetailModel()
                    {
                        Id = skillItem.Id,
                        Level = skillItem.Level * 10,
                        Name = skillItem.GetLocalized(x => x.Name),
                        LevelDescription = skillItem.GetLocalized(x => x.LevelDescription)
                    };
                    skillModel.SkillItems.Add(skillItemModel);
                }
                model.Skills.Add(skillModel);
            }
            #endregion

            #region Experience

            foreach (var experience in workHistory.Experiences.OrderByDescending(x => x.DisplayOrder))
            {
                var experienceModel = new WorkExperienceDetailsModels()
                {
                    Id = experience.Id,
                    Name = experience.GetLocalized(x => x.Name),
                    Period = experience.GetLocalized(x => x.Period),
                    Function = experience.GetLocalized(x => x.Function),
                    City = experience.GetLocalized(x => x.City),
                    //Tasks = Regex.Split(experience.GetLocalized(x => x.Tasks), "\r\n")
                };
                var tasks = experience.GetLocalized(x => x.Tasks);
                experienceModel.Tasks = tasks.Replace("\r\n", "<br >");

                foreach (var project in experience.Projects)
                {
                    var projectModel = new ProjectDetailModel()
                    {
                        Id = project.Id,
                        Name = project.GetLocalized(x => x.Name),
                        Description = project.GetLocalized(x => x.Description),
                        Technology = project.GetLocalized(x => x.Technology)
                    };
                    experienceModel.Projects.Add(projectModel);
                }
                model.Experiences.Add(experienceModel);
            }
            #endregion

            return model;
        }

        [NonAction]
        protected void UpdateLocales(Resume entity, ResumeModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEnitityHelperService.SaveLocalizedValue(entity,
                    x => x.Summary,
                    localized.Summary,
                    localized.LanguageId);
            }
        }

        #endregion

        #region Resume
        [HttpGet]
        public IActionResult Get()
        {
            var entities = _resumeService.GetAll();
            var model = entities.ToModels();
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var entity = _resumeService.GetById(id);
            var model = entity.ToModel();

            //locals
            AddLocales(_languageWebApi, model.Locales, (locale, languageId) =>
            {
                locale.Summary = entity.GetLocalized(x => x.Summary, languageId, false, false);
            });

            return Ok(model);
        }


       [HttpGet("details/{id}/{languageId}")]
        public IActionResult GetResumeDetails(int id, int languageId)
        {
            var current = _workContext.Current;
            current.WorkingLanguageId = languageId;
            _workContext.Current = current;
            //Including eductions, experiences and etc.
            var entity = _resumeService.GetDetails(id);
            var model = PrepareWorkHistoryDetailsModel(entity);

            return Ok(model);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ResumeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = model.ToEntity();
            entity.Id = 0;
            _resumeService.Insert(entity);

            //locales
            UpdateLocales(entity, model);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ResumeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = model.ToEntity();
            _resumeService.Update(entity);

            //locales
            UpdateLocales(entity, model);
            return NoContent();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var entity = _resumeService.GetById(id);
            _resumeService.Delete(entity);
            return RedirectToAction("Get");
        }

        #endregion      

        #region Import/Export
       
        [HttpPost("ImportXml")]
        public IActionResult ImportXml(string userName, IFormFile file)
        {

            if (file == null || file.Length == 0) return null;

            try
            {
                using (var sr = new StreamReader(file.OpenReadStream()))
                {
                    string content = sr.ReadToEnd();
                    //var currentUser = "test@test.nl";

                    var resume = _importManager.ImportWorkFromXml(content, userName);
                }
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException(exc.Message);
            }

            return NoContent();
        }

        [HttpGet("ExportToXml/{id}")]
        public async Task<IActionResult> ExportToXml(int id)
        {
            try
            {
                var entity = _resumeService.GetDetails(id);
                var xml = await _exportManager.ExportResumeToXml(entity);
                return File(Encoding.UTF8.GetBytes(xml), "application/xml", entity.Name + ".xml");
            }
            catch (Exception exc)
            {
                //ErrorNotification(exc, true);
                ModelState.AddModelError("ExportXml", exc.ToString());

                return RedirectToAction("Edit", new { id = id });
            }
        }


       

        [HttpGet("PrintToPdf/{id}")]
        public ActionResult PrintToPdf(int id)
        {
            try
            {
                var entity = _resumeService.GetDetails(id);

                byte[] bytes = null;
                using (var stream = new MemoryStream())
                {
                    _pdfService.PrintResume(stream, entity);
                    bytes = stream.ToArray();
                }
                return File(bytes, "application/pdf", string.Format("{0}_Resume.pdf", entity.Name));
            }
            catch (Exception)
            {
                //ErrorNotification(exc);
                return RedirectToAction("Index");
            }
        }

        #endregion
    }
}
