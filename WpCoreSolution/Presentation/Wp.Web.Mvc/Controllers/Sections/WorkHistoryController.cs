using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nsr.Common.Core;
using Nsr.RestClient;
using Nsr.RestClient.RestClients;
using Wp.Core;
using Wp.Core.Domain.Sections;
using Wp.Services.Sections;
using Wp.Services.WebPages;
using Wp.Web.Framework.UI;
using Wp.Web.Mvc.Extensions;
using Wp.Web.Mvc.Models.Sections;

namespace Wp.Web.Mvc.Controllers.Sections
{
    public class WorkHistoryController : SectionBaseController
    {
        // http://wrapbootstrap.com/preview/WB0265951 

        private ResiliencyHelper _resiliencyHelper;
        private readonly IWorkContext _workContext;
        private readonly IResumesWebApi _resumeManagementApi;
        private readonly ILogger<WorkHistoryController> _logger;

        public WorkHistoryController(IWebPageService webPageService, ISectionService sectionService, IWorkContext workContext, IWebHelper webHelper, IResumesWebApi resumeManagementApi, ILogger<WorkHistoryController> logger) : base(webPageService, sectionService, webHelper)
        {
            _workContext = workContext;
            _resumeManagementApi = resumeManagementApi;
            _logger = logger;
            _resiliencyHelper = new ResiliencyHelper(_logger);
        }

        #region Utilities

      
        #endregion

        public ActionResult ReadOnly(WorkHistorySectionReadOnlyModel model)
        {
            return PartialView("ReadOnly", model);
        }

       

        public ActionResult Edit(int id)
        {
            var resume = (WorkHistorySection)_sectionService.GetById(id);
            var model = resume.ToModel();

            return View(model);
        }

        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Edit(WorkHistorySectionModels model, SubmitType submitType)
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

                var entity = (WorkHistorySection)_sectionService.GetById(model.Id);
                entity = model.ToEntity(entity);
                _sectionService.Update(entity);
                SuccessNotification("Updated successfully. ", false);

            }
            return View(model);
        }
    }
}
