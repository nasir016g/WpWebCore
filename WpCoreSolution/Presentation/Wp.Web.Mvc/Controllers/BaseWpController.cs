﻿using Microsoft.AspNetCore.Mvc;
using Nsr.Common.Core.Localization.Models;
using Nsr.Common.Service.Localization;
using System;
using System.Collections.Generic;
using Wp.Web.Framework.UI;

namespace Wp.Web.Mvc.Controllers
{
    //[LanguageSeoCodeAttribute]
    public abstract partial class BaseWpController : Controller
    {
        protected string GetBasePath()
        {
            string basePath = HttpContext.Request.PathBase;
            if (basePath != "/")
                basePath += "/";
            return basePath;
        }
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }

        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }

        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
            if (exception.InnerException != null)
            {
                ErrorNotification(exception.InnerException, persistForTheNextRequest);
            }
        }

        private void AddNotification(NotifyType notifyType, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("notifications.{0}", notifyType);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }

        protected virtual void AddLocales<TLocalizedModelLocal>(ILanguageService languageService, IList<TLocalizedModelLocal> locales) where TLocalizedModelLocal : ILocalizedModelLocal
        {
            AddLocales(languageService, locales, null);
        }

        protected virtual void AddLocales<TLocalizedModelLocal>(ILanguageService languageService, IList<TLocalizedModelLocal> locales, Action<TLocalizedModelLocal, int> configure) where TLocalizedModelLocal : ILocalizedModelLocal
        {
            foreach (var language in languageService.GetAll())
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