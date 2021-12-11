using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nrs.RestClient;
using Nsr.Common.Core;
using Nsr.RestClient;
using Nsr.RestClient.Models.WorkHistories;
using Nsr.RestClient.RestClients;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wp.Web.Mvc.Extensions;

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


        #endregion        

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int languageId = _workContext.Current.WorkingLanguageId;
            var res = await _resiliencyHelper.ExecuteResilient(async () =>
            {
                var model = await _resumeManagementApi.GetResumeDetails(1, languageId);
                return model;
            }, null);


            return View(res);
        }
    }
}
