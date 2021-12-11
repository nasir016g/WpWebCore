using Nsr.RestClient.Models.Localization;
using Refit;

namespace Nsr.RestClient.RestClients.Localization
{
    public interface ILocalizationWebApi
    {
        [Get("/api/localization")]
        Task<List<LocaleStringResource>> GetAll();

        [Get("/api/localization/{id}")]
        Task<LocaleStringResource> GetById([AliasAs("id")] int id);

        [Get("/api/localization/GetAllResourceValues/{languageId}")]
        Task<List<LocaleStringResource>> GetAllResourceValues([AliasAs("languageId")] int languageId);

        [Get("/api/localization/GetResource/{resourceKey}")]
        Task<string> GetResource([AliasAs("resourceKey")] string resourceKey);

        [Get("/api/localization/GetResource/{resourceKey}/{languageId}/{defaultValue?}")]
        Task<string> GetResource([AliasAs("resourceKey")] string resourceKey, [AliasAs("languageId")] int languageId, [AliasAs("defaultValue")] string defaultValue = "");
       
        [Put("/api/localization/{id}")]
        Task Update(int id, LocaleStringResource model);

        [Delete("/api/localization/{id}")]
        Task Delete([AliasAs("id")] int id);        

      
    }
}
