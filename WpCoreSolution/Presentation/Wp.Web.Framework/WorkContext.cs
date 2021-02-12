//using Microsoft.AspNetCore.Http;
//using Nsr.Common.Core;
//using Nsr.Common.Services;
//using System.Linq;
//using Wp.Core;
//using Wp.Core.Domain.Common;
//using Wp.Web.Framework.Extensions;

//namespace Wp.Web.Framework
//{
//    public class WorkContext : IWorkContext
//    {
//        private readonly ILanguageService _languageService;
//        private readonly WebsiteSettings _websiteSettings;
//        private readonly IHttpContextAccessor _httpContextAccessor;

//        public WorkContext(ILanguageService languageService, WebsiteSettings websiteSettings, IHttpContextAccessor httpContextAccessor)
//        {
//            _languageService = languageService;
//            _websiteSettings = websiteSettings;
//            _httpContextAccessor = httpContextAccessor;
//        }

//        public WorkContext(WebsiteSettings websiteSettings, IHttpContextAccessor httpContextAccessor)
//        {
//            _websiteSettings = websiteSettings;
//            _httpContextAccessor = httpContextAccessor;
//        }


//        public WorkContextModel Current
//        {
//            get
//            {
//                WorkContextModel model = null;

//                if (_httpContextAccessor.HttpContext.Session != null)
//                {
//                    model = _httpContextAccessor.HttpContext.Session.GetObject<WorkContextModel>("__SessionManager__");
//                }

//                if (model == null)
//                {
//                    model = new WorkContextModel();
//                    model.WebSite = _websiteSettings;

//                    if (_languageService.GetAll().Count > 0)
//                    {
//                        model.WorkingLanguageId = _languageService.GetAll().Where(x => x.Published == true).FirstOrDefault().Id;
//                    }

//                    if (_httpContextAccessor.HttpContext.Session != null)
//                    {
//                        _httpContextAccessor.HttpContext.Session.SetObject("__SessionManager__", model);
//                    }
//                }
//                return model;
//            }

//            set
//            {
//                _httpContextAccessor.HttpContext.Session.SetObject("__SessionManager__", value);
//            }
//        }

//        public void ClearCurrentSession()
//        {
//            _httpContextAccessor.HttpContext.Session.Remove("__SessionManager__");
//        }
//    }
//}
