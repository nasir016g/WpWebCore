﻿using Microsoft.AspNetCore.Mvc;
using Nsr.Common.Core;
using Nsr.Common.Services;
using Nsr.RestClient.RestClients.Localization;
using System;
using Wp.Core;

namespace Wp.Web.Mvc.Controllers
{
    public class CommonController : Controller
    {
        private readonly ILanguageWebApi _languageWebApi;
        private readonly IWorkContext _workContext;
        //private readonly LocalizationSettings _localizationSettings;

        public CommonController(ILanguageWebApi languageWebApi, IWorkContext workContext)
        {
            _languageWebApi = languageWebApi;
            _workContext = workContext;
            //_localizationSettings = localizationSettings;
        }


        public IActionResult SetLanguage(int langid, string returnUrl = "")
        {
            var language = _languageWebApi.GetById(langid).GetAwaiter().GetResult();
            if (language != null && language.Published)
            {
                var current = _workContext.Current;
                current.WorkingLanguageId = language.Id;
                _workContext.Current = current;
                //_workContext.Current.WorkingLanguageId = language.Id;
            }

            //home page
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Url.RouteUrl("Home");

            if (!returnUrl.StartsWith("/Admin") && !returnUrl.StartsWith("/Work"))
            {
                ////language part in URL
                //if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
                //{
                //    //remove current language code if it's already localized URL
                //    if (returnUrl.IsLocalizedUrl(Request.PathBase, true, out var _))
                //        returnUrl = returnUrl.RemoveLanguageSeoCodeFromUrl(Request.PathBase, true);

                //    //and add code of passed language
                //    returnUrl = returnUrl.AddLanguageSeoCodeToUrl(Request.PathBase, true, language);
                //}
            }
            return Redirect(returnUrl);
        }
    }
}
