using Nsr.Common.Core.Localization;
using Nsr.Common.Core.Localization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Wp.Core;
using Microsoft.Extensions.DependencyInjection;
using Nsr.Common.Core;

namespace Wp.Web.Mvc.Extensions
{
    public static class LocalizationModelExtentions
    {
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
