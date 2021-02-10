using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Wp.Core;
using Wp.Core.Domain.Sections;
using Wp.Services.Sections;
using Wp.Services.WebPages;
using Wp.Web.Framework.UI;
using Wp.Web.Mvc.About.Models;
using Wp.Web.Mvc.About.RestClients;
using Wp.Web.Mvc.Helpers;
using Wp.Web.Mvc.Models.Resumes;
using Wp.Web.Mvc.Models.Sections;
using Nsr.Common.Services;
using System.Threading.Tasks;
using Wp.Web.Mvc.Extensions;
using Nsr.Common.Core;

namespace Wp.Web.Mvc.Controllers.Sections
{
    public class ResumeController : SectionBaseController
    {
        // http://wrapbootstrap.com/preview/WB0265951 

        private ResiliencyHelper _resiliencyHelper;
        private readonly IWorkContext _workContext;
        private readonly IResumesWebApi _resumeManagementApi;
        private readonly ILogger<ResumeController> _logger;

        public ResumeController(IWebPageService webPageService, ISectionService sectionService, IWorkContext workContext, IWebHelper webHelper, IResumesWebApi resumeManagementApi, ILogger<ResumeController> logger) : base(webPageService, sectionService, webHelper)
        {
            _workContext = workContext;
            _resumeManagementApi = resumeManagementApi;
            _logger = logger;
            _resiliencyHelper = new ResiliencyHelper(_logger);
        }

        #region Utilities

      
        #endregion

        public ActionResult ReadOnly(ResumeSectionReadOnlyModel model)
        {
            return PartialView("ReadOnly", model);
        }

       

        public ActionResult Edit(int id)
        {
            var resume = (ResumeSection)_sectionService.GetById(id);
            var model = resume.ToModel();

            return View(model);
        }

        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Edit(ResumeSectionModel model, SubmitType submitType)
        {
            var page = base._webPageService.GetById(model.WebPageId);

            if (submitType == SubmitType.Cancel)
            {
                return Redirect(GetBasePath() + page.VirtualPath);
            }

            if (submitType == SubmitType.Save)
            {
                if (!ModelState.IsValid)
                {
                    ErrorNotification("An error occured during updating.");
                    return View(model);
                }

                var entity = (ResumeSection)_sectionService.GetById(model.Id);
                entity = model.ToEntity(entity);
                _sectionService.Update(entity);
                SuccessNotification("Updated successfully. ", false);

            }
            return View(model);
        }
    }
}
