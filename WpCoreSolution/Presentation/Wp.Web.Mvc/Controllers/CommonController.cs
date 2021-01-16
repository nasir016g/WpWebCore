//using Microsoft.AspNetCore.Mvc;
//using System;
//using Wp.Core;
//using Wp.Core.Domain.Localization;
//using Wp.Services.Localization;

//namespace Wp.Web.Mvc.Controllers
//{
//    public class CommonController : Controller
//    {
//        private readonly ILanguageService _languageService;
//        private readonly IWorkContext _workContext;
//        private readonly LocalizationSettings _localizationSettings;

//        public CommonController(ILanguageService languageService, IWorkContext workContext, LocalizationSettings localizationSettings)
//        {
//            _languageService = languageService;
//            _workContext = workContext;
//            _localizationSettings = localizationSettings;
//        }
      

//        public IActionResult SetLanguage(int langid, string returnUrl = "")
//        {
//            var language = _languageService.GetById(langid);
//            if (language != null && language.Published)
//            {
//                _workContext.Current.WorkingLanguage = language;
//            }

//            //home page
//            if (String.IsNullOrEmpty(returnUrl))
//                returnUrl = Url.RouteUrl("Home");

//            if (!returnUrl.StartsWith("/Admin") && !returnUrl.StartsWith("/Work"))
//            {
//                //language part in URL
//                if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
//                {
//                    //remove current language code if it's already localized URL
//                    if (returnUrl.IsLocalizedUrl(Request.PathBase, true, out var _))
//                        returnUrl = returnUrl.RemoveLanguageSeoCodeFromUrl(Request.PathBase, true);

//                    //and add code of passed language
//                    returnUrl = returnUrl.AddLanguageSeoCodeToUrl(Request.PathBase, true, language);
//                }
//            }
//            return Redirect(returnUrl);
//        }
//    }
//}
