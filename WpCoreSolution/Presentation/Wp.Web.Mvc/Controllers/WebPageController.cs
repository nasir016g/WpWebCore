using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nsr.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wp.Core;
using Wp.Core.Domain.Sections;
using Wp.Core.Domain.WebPages;
using Wp.Services.Sections;
using Wp.Services.WebPages;
using Wp.Services.Websites;
using Wp.Web.Framework.Extensions.Mapper;
using Wp.Web.Mvc.Extensions;

namespace Wp.Web.Mvc.Controllers
{
    public class WebPageController : BaseWpController
    {
        private readonly IWebsiteService _websiteService;
        private readonly IWebPageService _webPageService;
        private readonly ISectionService _sectionService;
        private readonly IWorkContext _workContext;
        private readonly IWebHelper _webHelper;
        private readonly ILogger<WebPageController> _logger;

        public WebPageController(IWebsiteService websiteService, IWebPageService webPageService, ISectionService sectionService, IWorkContext workContext, IWebHelper webHelper, ILogger<WebPageController> logger)
        {
            _websiteService = websiteService;
            _webPageService = webPageService;
            _sectionService = sectionService;
            _workContext = workContext;
            _webHelper = webHelper;
            _logger = logger;
        }
        public ActionResult DetailsView(int webPageId)
        {
            _logger.LogInformation(string.Format("WebPage-DetailsView {0}", webPageId));
            WebPage entity = _webPageService.GetById(webPageId);
            if (entity != null)
            {
                if (!_webPageService.HasViewRights(entity.Id))
                    entity = null;
            }

            var languageId = _workContext.Current;

            if (entity == null)
                entity = _webPageService.GetAll().FirstOrDefault();

            if (entity == null)
                return RedirectToRoute("Install");

            //if (!string.IsNullOrWhiteSpace(entity.Theme))
            //    _workContext.Current.WebSite.Theme = entity.Theme;

            //var sections = page.Sections.ToList();
            var model = entity.ToModel(_webPageService, _sectionService);

            // meta's
            var website = _websiteService.GetAll().First();  //_workContext.Current.WebSite;

            ////customer attribute services
            //if (_customAttributeService != null && _customAttributeParser != null)
            //{
            //    //PrepareCustomWebPageAttributes(model, entity, _webPageAttributeService, _webPageAttributeParser, overrideAttributesXml);
            //}
            //if (addressAttributeFormatter != null && address != null)
            //{
            //    model.FormattedCustomAddressAttributes = addressAttributeFormatter.FormatAttributes(address.CustomAttributes);
            //}

            ViewBag.MetaTitle = !String.IsNullOrEmpty(entity.MetaTitle) ? entity.MetaTitle + " - " + website.Title : website.Title;
            ViewBag.MetaDescription = !String.IsNullOrEmpty(entity.MetaDescription) ? entity.MetaDescription : website.MetaDescription;
            ViewBag.MetaKeywords = !String.IsNullOrEmpty(entity.MetaKeywords) ? entity.MetaKeywords : website.MetaKeywords;

            return View(model);
        }

        public ActionResult AddSection(int pageId, IFormCollection collection)
        {
            var selectedValue = collection["selectedSection"];
            var page = _webPageService.GetById(pageId);

            Section section = null;
            switch (selectedValue.ToString().ToLower())
            {
                case "html":
                    section = new HtmlContentSection();
                    break;
                case "contactform":
                    section = new ContactFormSection { EmailTo = "your@email.com" };
                    break;
                case "resume":
                    section = new ResumeSection { ApplicationUserName = HttpContext.User.Identity.Name };
                    break;
            }

            section.WebPage = page;
            _sectionService.Insert(section);


            return Redirect(GetBasePath() + page.VirtualPath);
        }

       
    }
}
