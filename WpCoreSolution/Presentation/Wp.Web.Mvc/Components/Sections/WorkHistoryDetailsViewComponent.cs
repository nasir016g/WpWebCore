using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nsr.Common.Core;
using Nsr.Common.Services;
using Nsr.RestClient.Models.WorkHistories;
using Nsr.RestClient.RestClients;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wp.Web.Mvc.Extensions;
using Wp.Web.Mvc.Helpers;

namespace Wp.Web.Mvc.Components.Sections
{
    public class WorkHistoryDetailsViewComponent : ViewComponent
    {
        private readonly IResumesWebApi _resumeManagementApi;
        private readonly IWorkContext _workContext;
        private readonly ILogger<WorkHistoryDetailsViewComponent> _logger;
        private ResiliencyHelper _resiliencyHelper;

        public WorkHistoryDetailsViewComponent(IResumesWebApi resumeManagementApi, IWorkContext workContext, ILogger<WorkHistoryDetailsViewComponent> logger)
        {
            _resumeManagementApi = resumeManagementApi;
            _workContext = workContext;
            _logger = logger;
            _resiliencyHelper = new ResiliencyHelper(_logger);
        }

        #region Utilities

        protected WorkHistoryDetailsModel PrepareResDetailsModel(WorkHistoryDetailsModel workHistory)
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
