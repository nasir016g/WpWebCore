
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nsr.Common.Core;
using Nsr.Common.Service.Extensions;
using Nsr.RestClient.RestClients.Localization;

namespace Nsr.RestClient
{
    public class WorkContext : IWorkContext
    {
        private readonly ILanguageWebApi _languageWebApi;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<WorkContext> _logger;
        private ResiliencyHelper _resiliencyHelper;

        //public WorkContext(ILanguageWebApi languageWebApi, IHttpContextAccessor httpContextAccessor)
        //{
        //    _languageWebApi = languageWebApi;
        //    _httpContextAccessor = httpContextAccessor;
        //}

        public WorkContext(ILanguageWebApi languageWebApi, IHttpContextAccessor httpContextAccessor, ILogger<WorkContext> logger)
        {
            _languageWebApi = languageWebApi;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _resiliencyHelper = new ResiliencyHelper(_logger);
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

                   var languages = _languageWebApi.GetAll().GetAwaiter().GetResult();               


                    if (languages.Count > 0)
                    {
                        model.WorkingLanguageId = languages.Where(x => x.Published == true).FirstOrDefault().Id;
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
