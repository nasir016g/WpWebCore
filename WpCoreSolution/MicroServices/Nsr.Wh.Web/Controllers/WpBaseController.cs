using Microsoft.AspNetCore.Mvc;
using Nsr.Common.Core.Localization.Models;
using Nsr.Common.Services;
using Nsr.RestClient.RestClients.Localization;
using System;
using System.Collections.Generic;

namespace Nsr.Wh.Web.Controllers
{

    public class WpBaseController : ControllerBase
    {
        protected virtual void AddLocales<TLocalizedModelLocal>(ILanguageWebApi languageWebApi, IList<TLocalizedModelLocal> locales) where TLocalizedModelLocal : ILocalizedModelLocal
        {
            AddLocales(languageWebApi, locales, null);
        }

        protected virtual void AddLocales<TLocalizedModelLocal>(ILanguageWebApi languageWebApi, IList<TLocalizedModelLocal> locales, Action<TLocalizedModelLocal, int> configure) where TLocalizedModelLocal : ILocalizedModelLocal
        {           

            foreach (var language in languageWebApi.GetAll().GetAwaiter().GetResult())
            {
                var locale = Activator.CreateInstance<TLocalizedModelLocal>();
                locale.LanguageId = language.Id;
                if (configure != null)
                {
                    configure.Invoke(locale, locale.LanguageId);
                }
                locales.Add(locale);
            }
        }
    }
}
