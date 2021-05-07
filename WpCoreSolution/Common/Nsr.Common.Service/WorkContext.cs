using Microsoft.AspNetCore.Http;
using Nsr.Common.Core;
using Nsr.Common.Service.Extensions;
using Nsr.Common.Services;
using System.Linq;


namespace Nsr.Common.Service
{
    public class WorkContext : IWorkContext
    {
        private readonly ILanguageService _languageService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WorkContext(ILanguageService languageService, IHttpContextAccessor httpContextAccessor)
        {
            _languageService = languageService;
            _httpContextAccessor = httpContextAccessor;
        }

        public WorkContext(IHttpContextAccessor httpContextAccessor)
        {
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

                    if (_languageService.GetAll().Count > 0)
                    {
                        model.WorkingLanguageId = _languageService.GetAll().Where(x => x.Published == true).FirstOrDefault().Id;
                    }

                    if (_httpContextAccessor.HttpContext.Session != null)
                    {
                        _httpContextAccessor.HttpContext.Session.SetObject("__SessionManager__", model);
                    }
                }
                return model;
            }

            set
            {
                _httpContextAccessor.HttpContext.Session.SetObject("__SessionManager__", value);
            }
        }

        public void ClearCurrentSession()
        {
            _httpContextAccessor.HttpContext.Session.Remove("__SessionManager__");
        }
    }
}
