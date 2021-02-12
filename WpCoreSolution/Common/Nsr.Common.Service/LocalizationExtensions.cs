using Microsoft.Extensions.DependencyInjection;
using Nsr.Common.Core;
using Nsr.Common.Core.Localization;
using Nsr.Common.Core.Localization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Nsr.Common.Services
{
    public static class LocalizationExtensions
    {
        public static string GetLocalized<T>(this T entity, Expression<Func<T, string>> keySelector)
           where T : IEntity, ILocalizedEntity
        {
            using (var serviceScope = ServiceLocator.GetScope())
            {
                var workContext = serviceScope.ServiceProvider.GetService<IWorkContext>();
                return GetLocalized(entity, keySelector, workContext.Current.WorkingLanguageId);
            }            
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
                    using (var serviceScope = ServiceLocator.GetScope())
                    {
                        var languageService = serviceScope.ServiceProvider.GetService<ILanguageService>();
                        var totalPublishedLanguages = languageService.GetAll().Count;
                        loadLocalizedValue = totalPublishedLanguages >= 2;
                    }                        
                }

                //localized value
                if (loadLocalizedValue)
                {
                    using (var serviceScope = ServiceLocator.GetScope())
                    {
                        var localizedEntityService = serviceScope.ServiceProvider.GetService<ILocalizedEntityService>();
                        resultStr = localizedEntityService.GetLocalizedValue(languageId, entity.Id, localeKeyGroup, localeKey);
                        if (!String.IsNullOrEmpty(resultStr))
                            result = CommonHelper.To<TPropType>(resultStr);
                    } 
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


        // ILocalizedModel
        public static string GetLocalized<T, TLocalizedModelLocal>(this T model, IList<TLocalizedModelLocal> locales, Expression<Func<T, string>> keySelector)
            where T : ILocalizedModel<TLocalizedModelLocal>
            where TLocalizedModelLocal : ILocalizedModelLocal
        {

            if (model == null)
                throw new ArgumentNullException("model");

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
            string result = "";
            using (var serviceScope = ServiceLocator.GetScope())
            {
                var workContext = serviceScope.ServiceProvider.GetService<IWorkContext>();
                var local = locales.FirstOrDefault(x => x.LanguageId == workContext.Current.WorkingLanguageId);

                result = (string)local.GetType().GetProperty(propInfo.Name).GetValue(local, null);

            }

            //set default value if required
            if (String.IsNullOrEmpty(result))
            {
                var localizer = keySelector.Compile();
                result = localizer(model);
            }

            return result;
        }
    }
}
