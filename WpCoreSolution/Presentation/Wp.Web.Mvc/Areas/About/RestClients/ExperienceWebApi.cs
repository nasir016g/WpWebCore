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
    public class ExperienceWebApi : IExperienceWebApi
    {
        private IExperienceWebApi _restClient;
        public ExperienceWebApi(IConfiguration config, HttpClient httpClient)
        {
            string apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("ResumesWebApi");
            httpClient.BaseAddress = new Uri($"http://{apiHostAndPort}");
            _restClient = RestService.For<IExperienceWebApi>(httpClient);
        }

        public async Task<List<ExperienceModel>> GetExperiences([AliasAs("resumeId")] int resumeId)
        {
            return await _restClient.GetExperiences(resumeId);
        }

        public async Task<ExperienceModel> GetExperienceById([AliasAs("id")] int id)
        {
            try
            {
                return await _restClient.GetExperienceById(id);
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
        public async Task Update(int id, ExperienceModel model)
        {
            await _restClient.Update(id, model);
        }

        public async Task Delete([AliasAs("id")] int id)
        {
            await _restClient.Delete(id);
        }

        public async Task<List<ProjectModel>> GetItems(int experienceId)
        {
            return await _restClient.GetItems(experienceId);
        }

        public async Task<ProjectModel> GetItem(int id)
        {
            return await _restClient.GetItem(id);
        }

        public async Task UpdateItem(int id, ProjectModel model)
        {
            await _restClient.UpdateItem(id, model);
        }

        public async Task DeleteItem([AliasAs("id")] int id)
        {
            await _restClient.DeleteItem(id);
        }

    }
}
