using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Wp.Web.Mvc.Areas.Admin.Models;

namespace Wp.Web.Mvc.Areas.Admin.RESTClients
{
    public class ResumeManagementApi : IResumeManagementApi
    {
        private IResumeManagementApi _restClient;
        public ResumeManagementApi(IConfiguration config, HttpClient httpClient)
        {
            string apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("ResumeManagementApi");
            httpClient.BaseAddress = new Uri($"http://{apiHostAndPort}");
            _restClient = RestService.For<IResumeManagementApi>(httpClient);
        }

        public async Task<List<ResumeModel>> GetResume()
        {
            return await _restClient.GetResume();
        }
    }
}
