using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Wp.Core.Domain.Sections;
using Wp.Services.Localization;
using Wp.Services.Sections;
using Wp.Services.WebPages;
using Wp.Web.Api.Extensions.Mapper;

namespace Wp.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebPageController : ControllerBase
    {
        private readonly IWebPageService _webPageService;
        private readonly ISectionService _sectionService;
        private readonly ILocalizedEntityService _leService;

        public WebPageController(IWebPageService webPageService, ISectionService sectionService, ILocalizedEntityService leSerive)
        {
            _webPageService = webPageService;
            _sectionService = sectionService;
            _leService = leSerive;
        }

        //[HttpGet("{webPageId}", Name = "DetailsView")]
        //public ActionResult DetailsView(int webPageId)
        //{
        //    var entity = _webPageService.GetById(webPageId);
        //    if (entity != null)
        //    {
        //        if (!_webPageService.HasViewRights(entity.Id))
        //            entity = null;
        //    }

        //    if (entity == null)
        //        entity = _webPageService.GetAll().FirstOrDefault();

        //    if (entity == null)
        //        return RedirectToRoute("Install");

        //    //var sections = page.Sections.ToList();
        //    var model = entity.ToModel(_webPageService, _sectionService, User, 1, _leService); // Todo: Get language id from user profile


        //    ////customer attribute services
        //    //if (_customAttributeService != null && _customAttributeParser != null)
        //    //{
        //    //    //PrepareCustomWebPageAttributes(model, entity, _webPageAttributeService, _webPageAttributeParser, overrideAttributesXml);
        //    //}
        //    ////if (addressAttributeFormatter != null && address != null)
        //    ////{
        //    ////    model.FormattedCustomAddressAttributes = addressAttributeFormatter.FormatAttributes(address.CustomAttributes);
        //    ////}

        //    ////ViewBag.MetaTitle = !String.IsNullOrEmpty(entity.MetaTitle) ? entity.MetaTitle + " - " + website.Title : website.Title;
        //    ////ViewBag.MetaDescription = !String.IsNullOrEmpty(entity.MetaDescription) ? entity.MetaDescription : website.MetaDescription;
        //    ////ViewBag.MetaKeywords = !String.IsNullOrEmpty(entity.MetaKeywords) ? entity.MetaKeywords : website.MetaKeywords;

        //    return Ok(model);
        //}

        // temp: use UrlRecord later
        [HttpGet("{virtualPath}", Name = "DetailsViewByVirtualPath")]
        public ActionResult DetailsViewByVirtualPath(string virtualPath)
        {
            var entity = _webPageService.GetByVirtualPath(virtualPath);
            if (entity != null)
            {
                if (!_webPageService.HasViewRights(entity.Id))
                    entity = null;
            }

            if (entity == null)
                entity = _webPageService.GetAll().FirstOrDefault();

            if (entity == null)
                return RedirectToRoute("Install");

            //var sections = page.Sections.ToList();
            var model = entity.ToFrontEndModel(_webPageService, _sectionService, User, 1, _leService); // Todo: Get language id from user profile


            ////customer attribute services
            //if (_customAttributeService != null && _customAttributeParser != null)
            //{
            //    //PrepareCustomWebPageAttributes(model, entity, _webPageAttributeService, _webPageAttributeParser, overrideAttributesXml);
            //}
            ////if (addressAttributeFormatter != null && address != null)
            ////{
            ////    model.FormattedCustomAddressAttributes = addressAttributeFormatter.FormatAttributes(address.CustomAttributes);
            ////}

            ////ViewBag.MetaTitle = !String.IsNullOrEmpty(entity.MetaTitle) ? entity.MetaTitle + " - " + website.Title : website.Title;
            ////ViewBag.MetaDescription = !String.IsNullOrEmpty(entity.MetaDescription) ? entity.MetaDescription : website.MetaDescription;
            ////ViewBag.MetaKeywords = !String.IsNullOrEmpty(entity.MetaKeywords) ? entity.MetaKeywords : website.MetaKeywords;

            return Ok(model);
        }

        [HttpGet("{pageId}/{selectedValue}", Name = "AddSection")]
        public ActionResult AddSection(int pageId, string selectedValue)
        {
            var page = _webPageService.GetById(pageId);

            Section section = null;
            switch (selectedValue.ToLower())
            {
                case "htmlcontent":
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

            return NoContent();
        }
    }
}