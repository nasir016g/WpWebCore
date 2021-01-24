using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Wp.Web.Mvc.Models;

namespace Wp.Web.Mvc.RestClients
{
    public class ResumesWebApi : IResumesWebApi
    {
        private IResumesWebApi _restClient;
        public ResumesWebApi(IConfiguration config, HttpClient httpClient)
        {
            string apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("ResumesWebApi");
            httpClient.BaseAddress = new Uri($"http://{apiHostAndPort}");
            _restClient = RestService.For<IResumesWebApi>(httpClient);
        }

        public async Task<List<ResumeModel>> GetResume()
        {
            return await _restClient.GetResume();
        }
    }
}
