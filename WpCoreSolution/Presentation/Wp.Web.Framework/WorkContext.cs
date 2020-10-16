using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Core;
using Wp.Core.Domain.Common;
using Wp.Services.Localization;
using Wp.Web.Framework.Extensions;

namespace Wp.Web.Framework
{
    public class WorkContext : IWorkContext
    {
        private readonly ILanguageService _languageService;
        private readonly WebsiteSettings _websiteSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WorkContext(ILanguageService languageService, WebsiteSettings websiteSettings, IHttpContextAccessor httpContextAccessor)
        {
            _languageService = languageService;
            _websiteSettings = websiteSettings;
            _httpContextAccessor = httpContextAccessor;
        }

        public WorkContextModel Current
        {
            get
            {
                WorkContextModel model = null;

                if (_httpContextAccessor.HttpContext.Session != null)
                {
                    model = _httpContextAccessor.HttpContext.Session.GetObject<WorkContextModel>("__SessionManager__");
                }

                if (model == null)
                {
                    model = new WorkContextModel();
                    model.WebSite = _websiteSettings;
                    model.WorkingLanguage = _languageService.GetAll().Where(x => x.Published == true).FirstOrDefault();

                    if (_httpContextAccessor.HttpContext.Session != null)
                    {
                        _httpContextAccessor.HttpContext.Session.SetObject("__SessionManager__", model);
                    }
                }
                return model;
            }
        }
    }
}
