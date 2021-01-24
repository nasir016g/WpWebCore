using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Wp.Common;
using Wp.Localization.Core;

namespace Wp.Localization.Services
{
    public static class LocalizationExtensions
    {
        private static ILanguageService _languageService;
        private static ILocalizedEntityService _localizedEntityService;
        public static void Configure(ILanguageService languageService, ILocalizedEntityService localizedEntityService)
        {
            _languageService = languageService;
            _localizedEntityService = localizedEntityService;
        }
        public static string GetLocalized<T>(this T entity, Expression<Func<T, string>> keySelector)
           where T : IEntity, ILocalizedEntity
        {
            //var workContext = ServiceLocator.Instance.GetService<IWorkContext>();
            //return GetLocalized(entity, keySelector, workContext.Current.WorkingLanguage.Id);
            return GetLocalized(entity, keySelector, 1);
        }

        public static string GetLocalized<T>(this T entity, Expression<Func<T, string>> keySelector, int languageId, bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
            where T : IEntity, ILocalizedEntity
        {
            return GetLocalized<T, string>(entity, keySelector, languageId, returnDefaultValue, ensureTwoPublishedLanguages);
        }

        public static TPropType GetLocalized<T, TPropType>(this T entity,  Expression<Func<T, TPropType>> keySelector, int languageId, bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
           where T : IEntity, ILocalizedEntity
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

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

            TPropType result = default(TPropType);
            string resultStr = string.Empty;

            //load localized value
            string localeKeyGroup = typeof(T).Name;
            string localeKey = propInfo.Name;

            if (languageId > 0)
            {
                //ensure that we have at least two published languages
                bool loadLocalizedValue = true;
                if (ensureTwoPublishedLanguages)
                {
                    //var lService = ServiceLocator.Instance.GetService<ILanguageService>();
                    var totalPublishedLanguages = _languageService.GetAll().Count;
                    loadLocalizedValue = totalPublishedLanguages >= 2;
                }

                //localized value
                if (loadLocalizedValue)
                {
                    //var leService = ServiceLocator.Instance.GetService<ILocalizedEntityService>();
                    resultStr = _localizedEntityService.GetLocalizedValue(languageId, entity.Id, localeKeyGroup, localeKey);
                    if (!String.IsNullOrEmpty(resultStr))
                        result = CommonHelper.To<TPropType>(resultStr);
                }
            }

            //set default value if required
            if (String.IsNullOrEmpty(resultStr) && returnDefaultValue)
            {
                var localizer = keySelector.Compile();
                result = localizer(entity);
            }

            return result;
        }
    }
}
