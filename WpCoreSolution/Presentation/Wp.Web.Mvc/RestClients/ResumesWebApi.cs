using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
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

        public async Task<ResumeModel> GetResumeById([AliasAs("id")] int id)
        {
            try
            {
                return await _restClient.GetResumeById(id);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task Update(int id, ResumeModel model)
        {
            await _restClient.Update(id, model);
        }

        public async Task Delete([AliasAs("id")] int id)
        {
            await _restClient.Delete(id);
        }
    }
}
