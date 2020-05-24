using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wp.Core;
using Wp.Core.Domain.Common;
using Wp.Services.Localization;

namespace Wp.Web.Framework
{
    public class WorkContext : IWorkContext
    {
        private readonly ILanguageService _languageService;
        private readonly WebsiteSettings _websiteSettings;

        public WorkContext(ILanguageService languageService, WebsiteSettings websiteSettings)
        {
            _languageService = languageService;
            _websiteSettings = websiteSettings;
        }

        public WorkContextModel Current
        {
            get
            {
                WorkContextModel model = null;

                //if (HttpContext.Current.Session != null)
                //{
                //    model = (WorkContextModel)HttpContext.Current.Session["__SessionManager__"];
                //}

                if (model == null)
                {
                    model = new WorkContextModel();
                    model.WebSite = _websiteSettings;
                    model.WorkingLanguage = _languageService.GetAll().Where(x => x.Published == true).FirstOrDefault();

                    //if (HttpContext.Current.Session != null)
                    //{
                    //    HttpContext.Current.Session["__SessionManager__"] = model;
                    //}
                }
                return model;
            }
        }
    }
}
