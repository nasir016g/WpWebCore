using Wp.Web.Framework.Localization;

namespace Wp.Web.Framework.ViewEngines.Razor
{
    public abstract class WebViewPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>
    {
        //private ILocalizationService _localizationService;
        private Localizer _localizer;
       // private IWorkContext _workContext;

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
                        //var resFormat = _localizationService.GetResource(format);
                        string resFormat = "";
                        if (string.IsNullOrEmpty(resFormat))
                        {
                            return new LocalizedString(format);
                        }
                        return
                            new LocalizedString((args == null || args.Length == 0)
                                                    ? resFormat
                                                    : string.Format(resFormat, args));
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
