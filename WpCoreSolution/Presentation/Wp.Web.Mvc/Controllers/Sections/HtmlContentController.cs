using Microsoft.AspNetCore.Mvc;
using Nrs.RestClient;
using Nsr.Common.Services;
using Nsr.RestClient;
using Nsr.RestClient.RestClients.Localization;
using Wp.Core;
using Wp.Core.Domain.Sections;
using Wp.Services.Sections;
using Wp.Services.WebPages;
using Wp.Web.Framework.UI;
using Wp.Web.Mvc.Extensions;
using Wp.Web.Mvc.Models.Sections;

namespace Wp.Web.Mvc.Controllers.Sections
{
    public class HtmlContentController : SectionBaseController
    {
        private readonly ILanguageWebApi _languageWebApi;
        private readonly ILocalizedEntityWebApi _localizedEntityWebApi;
        private readonly ILocalizedEnitityHelperService _localizedEntityHelperService;

        public HtmlContentController(ILanguageWebApi languageWebApi, ILocalizedEntityWebApi localizedEntityWebApi, ILocalizedEnitityHelperService localizedEntityHelperService, IWebPageService webPageService, ISectionService sectionService, IWebHelper webHelper) : base(webPageService, sectionService, webHelper)
        {
            _languageWebApi = languageWebApi;
            _localizedEntityWebApi = localizedEntityWebApi;
            _localizedEntityHelperService = localizedEntityHelperService;
        }

        #region Utilities

        [NonAction]
        protected void UpdateLocales(HtmlContentSection htmlContent, HtmlContentSectionModel model)
        {
            foreach(var localized in model.Locales)
            {
                _localizedEntityHelperService.SaveLocalizedValue(htmlContent,
                                                                x => x.Html,
                                                                localized.Html,
                                                                localized.LanguageId);
            }
        }

        #endregion

        public ActionResult ReadOnly(HtmlContentSectionReadOnlyModel model)
        { 
          return PartialView("ReadOnly", model);
        }

        public ActionResult Edit(int id)
        {
            var htmlContent = (HtmlContentSection)_sectionService.GetById(id);
            var model = htmlContent.ToModel();

            //locales
            AddLocales(_languageWebApi, model.Locales, (locale, languageId) =>
                {
                    locale.Html = htmlContent.GetLocalized(x => x.Html, languageId, false, false);
                });

            //AddDynamicProperties(IHasDynamicProperties owner);


           return View(model);
        }

        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Edit(HtmlContentSectionModel model, SubmitType submitType)
        {            
          var page = _webPageService.GetById(model.WebPageId);

            if(submitType == SubmitType.Cancel)
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

            var entity = (HtmlContentSection) _sectionService.GetById(model.Id);
            entity = model.ToEntity(entity);
            _sectionService.Update(entity);           

            //locales
            UpdateLocales(entity, model);

            SuccessNotification("Updated successfully. ", false);
           
          }
          return View(model);
        }

    }
}
