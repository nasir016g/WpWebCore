using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Wp.Core;
using Wp.Core.Common;
using Wp.Services.Localization;

namespace Wp.Services
{
    public static class Extensions
    {
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj, ILocalizationService localizationService, IWorkContext workContext,
           bool markCurrentAsSelected = true, int[] valuesToExclude = null, bool useLocalization = true) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("An Enumeration type is required.", "enumObj");

            //var localizationService = DependencyResolver.Current.GetService<ILocalizationService>();
            //var workContext = DependencyResolver.Current.GetService<IWorkContext>();

            var values = from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                         where valuesToExclude == null || !valuesToExclude.Contains(Convert.ToInt32(enumValue))
                         select new { ID = Convert.ToInt32(enumValue), Name = CommonHelper.ConvertEnum(enumValue.ToString()) };
            object selectedValue = null;
            if (markCurrentAsSelected)
                selectedValue = Convert.ToInt32(enumObj);
            return new SelectList(values, "ID", "Name", selectedValue);
        }

        public static SelectList ToSelectList<T>(this T objList, Func<Entity, string> selector) where T : IEnumerable<Entity>
        {
            return new SelectList(objList.Select(p => new { ID = p.Id, Name = selector(p) }), "ID", "Name");
        }
    }
}
