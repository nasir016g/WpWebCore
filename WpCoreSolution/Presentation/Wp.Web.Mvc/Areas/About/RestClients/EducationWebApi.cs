using Microsoft.Extensions.Configuration;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Wp.Web.Mvc.About.Models;

namespace Wp.Web.Mvc.About.RestClients
{
    public class EducationWebApi : IEducationWebApi
    {
        private IEducationWebApi _restClient;
        public EducationWebApi(IConfiguration config, HttpClient httpClient)
        {
            string apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("ResumesWebApi");
            httpClient.BaseAddress = new Uri($"http://{apiHostAndPort}");
            _restClient = RestService.For<IEducationWebApi>(httpClient);
        }
       
        public async Task<List<EducationAdminModel>> GetEducations([AliasAs("resumeId")] int resumeId)
        {
            return await _restClient.GetEducations(resumeId);
        }

        public async Task<EducationAdminModel> GetEducationById([AliasAs("id")] int id)
        {
            try
            {
                return await _restClient.GetEducationById(id);
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
       

        public async Task Update(int id, EducationAdminModel model)
        {
            await _restClient.Update(id, model);
        }

        public async Task Delete([AliasAs("id")] int id)
        {
            await _restClient.Delete(id);
        }

        public async Task<List<EducationItemAdminModel>> GetItems(int educationId)
        {
            return await _restClient.GetItems(educationId);
        }

        public async Task<EducationItemAdminModel> GetItem(int id)
        {
            return await _restClient.GetItem(id);
        }

        public async Task UpdateItem(int id, EducationItemAdminModel model)
        {
            await _restClient.UpdateItem(id, model);
        }

        public async Task DeleteItem([AliasAs("id")] int id)
        {
            await _restClient.DeleteItem(id);
        }
    }
}
