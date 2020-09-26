using Microsoft.AspNetCore.Html;
using System;
using System.Web;

namespace Wp.Web.Framework.Localization
{
    public class LocalizedString : HtmlString
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="localized">Localized value</param>
        public LocalizedString(string localized) : base(localized)
        {
            Text = localized;
        }

        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; }
    }
}
