using Nsr.RestClient.Models.Localization;
using Refit;

namespace Nsr.RestClient.RestClients.Localization
{
    public interface ILocalizedEntityWebApi
    {
        //[Get("/localizedEntity/GetLocalizedPropertyById/{id}")]
        //Task<LocalizedProperty> GetLocalizedPropertyById([AliasAs("id")] int id);

        //[Get("/localizedEntity/GetEntityIdByLocalizedValue/{localeKeyGroup}/{localeKey}/{localeValue}")]
        //Task<int> GetEntityIdByLocalizedValue(string localeKeyGroup, string localeKey, string localeValue);

        [Get("/api/localizedEntity/GetLocalizedValue/{languageId}/{entityId}/{localeKeyGroup}/{localeKey}")]
        Task<string> GetLocalizedValue(int languageId, int entityId, string localeKeyGroup, string localeKey);

        [Get("/api/localizedEntity/GetLocalizedProperties/{entityId}/{localeKeyGroup}")]
        Task<IList<LocalizedProperty>> GetLocalizedProperties(int entityId, string localeKeyGroup);


        [Post("/api/localizedEntity")]
        Task InsertLocalizedProperty(LocalizedProperty localizedProperty);

        [Put("/api/localizedEntity/{id}")]
        Task UpdateLocalizedProperty(int id, LocalizedProperty localizedProperty);
        
        [Delete("/api/localizedEntity/{id}")]
        Task DeleteLocalizedProperty(int id);

    }
}
