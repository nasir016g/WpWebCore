using Microsoft.AspNetCore.Mvc;
using System;
using Wp.Core;
using Wp.Services.Sections;
using Wp.Services.WebPages;
using Wp.Web.Framework.UI;

namespace Wp.Web.Mvc.Controllers.Sections
{
    public class SectionBaseController : BaseWpController
    {
        protected readonly IWebPageService _webPageService;
        protected readonly ISectionService _sectionService;
        protected readonly IWebHelper _webHelper;

        public SectionBaseController(IWebPageService webPageService, ISectionService sectionService, IWebHelper webHelper)
        {
            _webPageService = webPageService;
            _sectionService = sectionService;
            _webHelper = webHelper;
        }
      public ActionResult Delete(int id)
      {          
          return View();
      }

      [HttpPost]
      public ActionResult Delete(int id, SubmitType submitType)
      {
        var section = _sectionService.GetById(id);
        var page = _webPageService.GetById(section.WebPageId);

        if (submitType == SubmitType.Delete)
        { 
            try
            {
                _sectionService.Delete(section);
            }
            catch(Exception ex)
            {
                ErrorNotification(ex, false);
                return View();
            }
           
            SuccessNotification("Section deleted successfully.");
        }

        //return Redirect(_webHelper.ApplicationPath + page.VirtualPath);
            return Redirect(page.VirtualPath);
        }
    }
}
