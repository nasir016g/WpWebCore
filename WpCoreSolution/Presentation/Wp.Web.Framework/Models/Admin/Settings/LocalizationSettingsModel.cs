using Nrs.RestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wp.Web.Framework;

namespace Wp.Web.Framework.Models.Admin
{
    public class LocalizationSettingsModel
    {
        [WpResourceDisplayName("Admin.Configuration.Settings.Localization.UseImagesForLanguageSelection")]
        public bool UseImagesForLanguageSelection { get; set; }

        [WpResourceDisplayName("Admin.Configuration.Settings.Localization.SeoFriendlyUrlsForLanguagesEnabled")]
        public bool SeoFriendlyUrlsForLanguagesEnabled { get; set; }

        [WpResourceDisplayName("Admin.Configuration.Settings.Localization.LoadAllLocaleRecordsOnStartup")]
        public bool LoadAllLocaleRecordsOnStartup { get; set; } 
    }
}