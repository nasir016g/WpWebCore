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
    public class SkillWebApi : ISkillWebApi
    {
        private ISkillWebApi _restClient;
        public SkillWebApi(IConfiguration config, HttpClient httpClient)
        {
            string apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("ResumesWebApi");
            httpClient.BaseAddress = new Uri($"http://{apiHostAndPort}");
            _restClient = RestService.For<ISkillWebApi>(httpClient);
        }
        public async Task<List<SkillAdminModel>> GetSkills([AliasAs("resumeId")] int resumeId)
        {
            return await _restClient.GetSkills(resumeId);
        }

        public async Task<SkillAdminModel> GetSkillById([AliasAs("id")] int id)
        {
            try
            {
                return await _restClient.GetSkillById(id);
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

        public async Task Update(int id, SkillAdminModel model)
        {
            await _restClient.Update(id, model);
        }

        public async Task Delete([AliasAs("id")] int id)
        {
            await _restClient.Delete(id);

        }

        public async Task<List<SkillItemAdminModel>> GetItems(int skillId)
        {
            return await _restClient.GetItems(skillId);
        }

        public async Task<SkillItemAdminModel> GetItem(int id)
        {
            return await _restClient.GetItem(id);
        }

        public async Task UpdateItem(int id, SkillItemAdminModel model)
        {
            await _restClient.UpdateItem(id, model);
        }

        public async Task DeleteItem([AliasAs("id")] int id)
        {
            await _restClient.DeleteItem(id);
        }
       
    }
}
