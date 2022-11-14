using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Nsr.Common.Core;
using Nsr.Common.Service.Localization;
using Wp.Web.Framework.Localization;

namespace Wp.Web.Framework.ViewEngines.Razor
{
    public abstract class WebViewPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>
    {
        [RazorInject]
        public ILocalizationService LocalizationService { get; set; }

        [RazorInject]
        public IWorkContext WorkContext { get; set; }

        private Localizer _localizer;

        /// <summary>
        /// Get a localized resources
        /// </summary>
        public Localizer T
        {
            get
            {
                //if (_localizationService == null)
                //    _localizationService = ServiceLocator.Instance.GetService<ILocalizationService>();
                if (_localizer == null)
                {

                    //default localizer
                    _localizer = (format, args) =>
                    {
                        // using (var serviceScope = Wp.Localization.Core.ServiceLocator.GetScope())
                        //{
                        //   var localizationService = serviceScope.ServiceProvider.GetService<ILocalizationService>();
                        var resFormat = LocalizationService.GetResource(format, WorkContext.Current.WorkingLanguageId);
                        if (string.IsNullOrEmpty(resFormat))
                        {
                            return new LocalizedString(format);
                        }
                        return
                            new LocalizedString((args == null || args.Length == 0)
                                                    ? resFormat
                                                    : string.Format(resFormat, args));
                        //}

                    };
                }
                return _localizer;
            }
        }
    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}
