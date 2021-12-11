using Nsr.Common.Core;
using Nsr.RestClient.Models.Localization;
using System.Linq.Expressions;

namespace Nsr.Localization.Web.Api.Services
{
    public interface ILocalizedEntityService
    {
        LocalizedProperty GetLocalizedPropertyById(int localizedPropertyId);
        int GetEntityIdByLocalizedValue(string localeKeyGroup, string localeKey, string localeValue);
        string GetLocalizedValue(int languageId, int entityId, string localeKeyGroup, string localeKey);
        IList<LocalizedProperty> GetLocalizedProperties(int entityId, string localeKeyGroup);

        void SaveLocalizedValue<T>(T entity,
            Expression<Func<T, string>> keySelector,
            string localeValue,
            int languageId) where T : IEntity, ILocalizedEntity;

        void SaveLocalizedValue<T, TPropType>(T entity,
           Expression<Func<T, TPropType>> keySelector,
           TPropType localeValue,
           int languageId) where T : IEntity, ILocalizedEntity;

        void DeleteLocalizedProperty(LocalizedProperty localizedProperty);
        void InsertLocalizedProperty(LocalizedProperty localizedProperty);
        void UpdateLocalizedProperty(LocalizedProperty localizedProperty);
    }
}