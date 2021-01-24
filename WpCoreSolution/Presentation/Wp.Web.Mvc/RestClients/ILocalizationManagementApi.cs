//using Refit;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Wp.Web.Mvc.Models;

//namespace Wp.Web.Mvc.RestClients
//{
//    public interface ILocalizationManagementApi
//    {
//        [Get("/language")]
//        Task<List<LanguageModel>> GetLanguages();

//        [Get("/language/{id}")]
//        Task<LanguageModel> GetLanguageById([AliasAs("id")] int id);

//        [Put("/language/{id}")]
//        Task Update(int id, LanguageModel model);

//        [Delete("/language")]
//        Task Delete([AliasAs("id")] int id);


//        [Get("/localization/language/{languageId}")]
//        Task<List<LanguageResourceModel>> GetResources([AliasAs("languageId")] int languageId);

//        [Get("/localization/{id}")]
//        Task<LanguageResourceModel> GetResourceById([AliasAs("id")] int id);

//        [Put("/localization/{id}")]
//        Task UpdateResource(int id, LanguageResourceModel model);

//        [Delete("/localization")]
//        Task DeleteResource([AliasAs("id")] int id);

//    }
//}
