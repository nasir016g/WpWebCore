//using Microsoft.Extensions.Configuration;
//using Refit;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using Wp.Web.Mvc.Models;

//namespace Wp.Web.Mvc.RestClients
//{
//    public class LocalizationManagementApi : ILocalizationManagementApi
//    {
//        private ILocalizationManagementApi _restClient;
//        public LocalizationManagementApi(IConfiguration config, HttpClient httpClient)
//        {
//            string apiHostAndPort = config.GetSection("APIServiceLocations").GetValue<string>("LocalizationManagementApi");
//            httpClient.BaseAddress = new Uri($"http://{apiHostAndPort}");
//            _restClient = RestService.For<ILocalizationManagementApi>(httpClient);
//        }

//        #region Language
//        public async Task<List<LanguageModel>> GetLanguages()
//        {
//            return await _restClient.GetLanguages();
//        }

//        public async Task<LanguageModel> GetLanguageById([AliasAs("id")] int id)
//        {
//            try
//            {
//                return await _restClient.GetLanguageById(id);
//            }
//            catch (ApiException ex)
//            {
//                if (ex.StatusCode == HttpStatusCode.NotFound)
//                {
//                    return null;
//                }
//                else
//                {
//                    throw;
//                }
//            }
//        }

//        public async Task Update(int id, LanguageModel model)
//        {
//            await _restClient.Update(id, model);
//        }

//        public async Task Delete([AliasAs("id")] int id)
//        {
//            await _restClient.Delete(id);
//        }

//        #endregion

//        #region Resources
//        public async Task<List<LanguageResourceModel>> GetResources([AliasAs("languageId")] int languageId)
//        {
//            return await _restClient.GetResources(languageId);
//        }

//        public async Task<LanguageResourceModel> GetResourceById([AliasAs("id")] int id)
//        {
//            try
//            {
//                return await _restClient.GetResourceById(id);
//            }
//            catch (ApiException ex)
//            {
//                if (ex.StatusCode == HttpStatusCode.NotFound)
//                {
//                    return null;
//                }
//                else
//                {
//                    throw;
//                }
//            }
//        }

//        public async Task UpdateResource(int id, LanguageResourceModel model)
//        {
//            await _restClient.UpdateResource(id, model);
//        }

//        public async Task DeleteResource([AliasAs("id")] int id)
//        {
//            await _restClient.DeleteResource(id);
//        }

//        #endregion
//    }
//}
