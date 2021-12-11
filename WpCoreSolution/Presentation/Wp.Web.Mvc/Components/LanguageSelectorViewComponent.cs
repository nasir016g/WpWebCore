using Microsoft.AspNetCore.Mvc;
using Nsr.Common.Core;
using Nsr.Common.Services;
using Nsr.RestClient.RestClients.Localization;
using System.Linq;
using Wp.Core;
using Wp.Web.Mvc.Models;

namespace Wp.Web.Mvc.Components
{
    public class LanguageSelectorViewComponent : ViewComponent
    {
        private readonly ILanguageWebApi _languageWebApi;
        private readonly IWorkContext _workContext;
        //private readonly LocalizationSettings _localizationSettings;

        public LanguageSelectorViewComponent(ILanguageWebApi languageWebApi,
            IWorkContext workContext)
        {
            _languageWebApi = languageWebApi;
            _workContext = workContext;
            //_localizationSettings = localizationSettings;
        }

        private LanguageSelectorModel PrepareLanguageSelectorModel()
        {
            var lans = _languageWebApi.GetAll().GetAwaiter().GetResult();
            var availableLanguages = _languageWebApi
                    .GetAll().GetAwaiter().GetResult()
                    .Select(x => new LanguageSelectorModel.LanguageModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        FlagImageFileName = x.FlagImageFileName,
                    })
                    .ToList();

            var model = new LanguageSelectorModel();
            if (availableLanguages.Count() > 0)
            {
                model.CurrentLanguageId = _workContext.Current.WorkingLanguageId;
                model.AvailableLanguages = availableLanguages;
                model.UseImages = false; //_localizationSettings.UseImagesForLanguageSelection;
            }

            return model;
        }

        public IViewComponentResult Invoke()
        {
            var model = PrepareLanguageSelectorModel();
            return View(model);
        }
    }
}
