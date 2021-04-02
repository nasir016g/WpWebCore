using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nsr.Common.Core;
using Nsr.Common.Services;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wp.Web.Mvc.AboutMe.RestClients;
using Wp.Web.Mvc.Extensions;
using Wp.Web.Mvc.Helpers;
using Wp.Web.Mvc.Models.Resumes;

namespace Wp.Web.Mvc.Components.Sections
{
    public class ResumeDetailsViewComponent : ViewComponent
    {
        private readonly IResumesWebApi _resumeManagementApi;
        private readonly IWorkContext _workContext;
        private readonly ILogger<ResumeDetailsViewComponent> _logger;
        private ResiliencyHelper _resiliencyHelper;

        public ResumeDetailsViewComponent(IResumesWebApi resumeManagementApi, IWorkContext workContext, ILogger<ResumeDetailsViewComponent> logger)
        {
            _resumeManagementApi = resumeManagementApi;
            _workContext = workContext;
            _logger = logger;
            _resiliencyHelper = new ResiliencyHelper(_logger);
        }

        #region Utilities

        protected ResumeModel PrepareResDetailsModel(ResumeModel Resume)
        {
            var model = new ResumeModel()
            {
                Id = Resume.Id,

                Name = Resume.Name,
                Address = Resume.Address,
                PostalCode = Resume.PostalCode,
                Town = Resume.Town,
                Phone = Resume.Phone,
                Mobile = Resume.Mobile,
                Email = Resume.Email,
                DateOfBirth = Resume.DateOfBirth,
                Website = Resume.Website
            };
            var summary = Resume.GetLocalized(x => x.Summary);

            if (!String.IsNullOrWhiteSpace(summary))
            {
                model.Summary = summary.Replace("\r\n", "<br />");
                model.Summary = model.Summary.Replace("\n", "<br />");
            }

            #region Education

            foreach (var education in Resume.Educations)
            {
                var educationModel = new EducationModel()
                {
                    Id = education.Id,
                    Name = education.GetLocalized(x => x.Name)
                };

                foreach (var educationItem in education.EducationItems)
                {
                    var educationItemModel = new EducationItemModel()
                    {
                        Id = educationItem.Id,
                        Name = educationItem.GetLocalized(x => x.Name),
                        //Descriptions = Regex.Split(educationItem.GetLocalized(x => x.Description), "\r\n").ToList(),
                        Period = educationItem.GetLocalized(x => x.Period),
                        Place = educationItem.GetLocalized(x => x.Place)
                    };
                    var description = Wp.Core.Common.HtmlHelper.StripTags(educationItem.GetLocalized(x => x.Description));
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
            var skills = Resume.Skills;
            foreach (var skill in skills.OrderBy(x => x.DisplayOrder))
            {
                var skillModel = new SkillModel()
                {
                    Id = skill.Id,
                    Name = skill.GetLocalized(x => x.Name)
                };

                foreach (var skillItem in skill.SkillItems.OrderByDescending(x => x.Level))
                {
                    var skillItemModel = new SkillItemModel()
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

            foreach (var experience in Resume.Experiences.OrderByDescending(x => x.DisplayOrder))
            {
                var experienceModel = new ExperienceModel()
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
                    var projectModel = new ProjectModel()
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

        #endregion

        

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var res = await _resiliencyHelper.ExecuteResilient(async () =>
            {
                var model = await _resumeManagementApi.GetResumeDetails(1);
                return model;
            }, null);

            var model = PrepareResDetailsModel(res);

            return View(model);
        }
    }
}
