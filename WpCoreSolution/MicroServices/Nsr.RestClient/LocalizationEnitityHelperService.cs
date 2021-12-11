using Nsr.Common.Core;
using Nsr.RestClient.Models.Localization;
using Nsr.RestClient.RestClients.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nsr.RestClient
{

    public interface ILocalizedEnitityHelperService
    {
        void SaveLocalizedValue<T>(T entity,
                    Expression<Func<T, string>> keySelector,
                    string localeValue,
                    int languageId) where T : IEntity, ILocalizedEntity;

        void SaveLocalizedValue<T, TPropType>(T entity,
           Expression<Func<T, TPropType>> keySelector,
           TPropType localeValue,
           int languageId) where T : IEntity, ILocalizedEntity;
    }
    public class LocalizedEnitityHelperService : ILocalizedEnitityHelperService
    {
        private readonly ILocalizedEntityWebApi _localizedEntityWebApi;

        public LocalizedEnitityHelperService(ILocalizedEntityWebApi localizedEntityWebApi)
        {
            _localizedEntityWebApi = localizedEntityWebApi;
        }
        public void SaveLocalizedValue<T>(T entity, Expression<Func<T, string>> keySelector, string localeValue, int languageId)
         where T : IEntity, ILocalizedEntity
        {
            SaveLocalizedValue<T, string>(entity, keySelector, localeValue, languageId);
        }

        public virtual void SaveLocalizedValue<T, TPropType>(T entity, Expression<Func<T, TPropType>> keySelector, TPropType localeValue, int languageId)
            where T : IEntity, ILocalizedEntity
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (languageId == 0)
                throw new ArgumentOutOfRangeException("languageId", "Language ID should not be 0");

            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }

            string localeKeyGroup = typeof(T).Name;
            string localeKey = propInfo.Name;

            var props = _localizedEntityWebApi.GetLocalizedProperties(entity.Id, localeKeyGroup).GetAwaiter().GetResult();
            var prop = props.FirstOrDefault(lp => lp.LanguageId == languageId &&
                lp.LocaleKey.Equals(localeKey, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            string localeValueStr = CommonHelper.To<string>(localeValue);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(localeValueStr))
                {
                    //delete
                    _localizedEntityWebApi.DeleteLocalizedProperty(prop.Id).GetAwaiter().GetResult();
                }
                else
                {
                    //update
                    prop.LocaleValue = localeValueStr;
                    _localizedEntityWebApi.UpdateLocalizedProperty(prop.Id, prop).GetAwaiter().GetResult();
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(localeValueStr))
                {
                    //insert
                    prop = new LocalizedProperty()
                    {
                        EntityId = entity.Id,
                        LanguageId = languageId,
                        LocaleKey = localeKey,
                        LocaleKeyGroup = localeKeyGroup,
                        LocaleValue = localeValueStr
                    };
                    _localizedEntityWebApi.InsertLocalizedProperty(prop).GetAwaiter().GetResult();
                }
            }
        }

    }
}
