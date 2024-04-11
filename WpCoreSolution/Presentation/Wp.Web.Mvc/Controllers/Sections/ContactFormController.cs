using Microsoft.AspNetCore.Mvc;
using Nsr.Common.Service.Localization;
using Wp.Core.Domain.Sections;
using Wp.Core;
using Wp.Services.Sections;
using Wp.Services.WebPages;
using Wp.Web.Framework.UI;
using Wp.Web.Mvc.Models.Sections;
using Wp.Web.Mvc.Extensions;
using Nsr.Common.Services;

namespace Wp.Web.Mvc.Controllers.Sections
{
    public class ContactFormController : SectionBaseController
    {
        private readonly ILanguageService _languageService;
        private readonly ILocalizedEntityService _localizedEntityService;

        public ContactFormController(ILanguageService languageService, ILocalizedEntityService localizedEntityService, IWebPageService webPageService, ISectionService sectionService, IWebHelper webHelper) : base(webPageService, sectionService, webHelper)
        {
            _languageService = languageService;
            _localizedEntityService = localizedEntityService;
        }

        #region Utilities

        [NonAction]
        protected void UpdateLocales(ContactFormSection contactForm, ContactFormSectionModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(contactForm,
                                                            x => x.Subject,
                                                            localized.Subject,
                                                            localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(contactForm,
                                                            x => x.IntroText,
                                                            localized.IntroText,
                                                            localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(contactForm,
                                                            x => x.ThankYouText,
                                                            localized.ThankYouText,
                                                            localized.LanguageId);
            }
        }

        #endregion

        [HttpPost]
        public ActionResult Submit(ContactFormSectionReadOnlyModel model)
        {
            var section = (ContactFormSection)base._sectionService.GetById(model.Id);

            if (!ModelState.IsValid)
            {
                ErrorNotification("An error occured while submitting contact form.");
                // model.WebPage = section.WebPage;
                return PartialView("ReadOnly", model);
                // return Redirect("~/" + section.WebPage.VirtualPath);
            }

            //IUserMailer userMailer = new UserMailer();
            //userMailer.ContactForm(model, section).Send();
            ViewBag.ThankYouText = section.ThankYouText;

            return PartialView("ThankYou");
        }

        public ActionResult ReadOnly(HtmlContentSectionReadOnlyModel model)
        {
            return PartialView("ReadOnly", model);
        }

        public ActionResult Edit(int id)
        {
            var contactForm = (ContactFormSection)base._sectionService.GetById(id);
            var model = contactForm.ToModel();

            //locales
            AddLocales(_languageService, model.Locales, (local, languageId) =>
            {
                local.Subject = contactForm.GetLocalized(x => x.Subject, languageId, false, false);
                local.IntroText = contactForm.GetLocalized(x => x.IntroText, languageId, false, false);
                local.ThankYouText = contactForm.GetLocalized(x => x.ThankYouText, languageId, false, false);
            });
            return View(model);
        }

        [HttpPost]
        //[ValidateInput(false)]
        public ActionResult Edit(ContactFormSectionModel model, SubmitType submitType)
        {
            var page = _webPageService.GetById(model.WebPageId);

            if (submitType == SubmitType.Cancel)
            {
                return Redirect(_webHelper.ApplicationPath + page.VirtualPath);
            }

            if (submitType == SubmitType.Save)
            {
                if (!ModelState.IsValid)
                {
                    ErrorNotification("An error occured during updating of Contact form.");
                    return View(model);
                }

                var entity = (ContactFormSection)_sectionService.GetById(model.Id);
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
