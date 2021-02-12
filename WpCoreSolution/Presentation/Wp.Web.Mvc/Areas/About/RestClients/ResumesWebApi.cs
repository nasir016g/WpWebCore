using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Wp.Web.Mvc.About.Models;
using Wp.Web.Mvc.Models.Resumes;

namespace Wp.Web.Mvc.About.RestClients
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
        
        
        public async Task<List<ResumeAdminModel>> GetResume()
        {
            return await _restClient.GetResume();
        }

        public async Task<ResumeAdminModel> GetResumeById([AliasAs("id")] int id)
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

        public async Task Update(int id, ResumeAdminModel model)
        {
            await _restClient.Update(id, model);
        }

        public async Task Delete([AliasAs("id")] int id)
        {
            await _restClient.Delete(id);
        }

        public async Task<ResumeModel> GetResumeDetails([AliasAs("id")] int id)
        {
            try
            {
                return await _restClient.GetResumeDetails(id);
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
    }
}
