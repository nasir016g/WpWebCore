using Microsoft.AspNetCore.Mvc;
using Nsr.Common.Core;
using Nsr.Common.Service.Localization;
using System.Linq;
using Wp.Web.Mvc.Models;

namespace Wp.Web.Mvc.Components
{
    public class LanguageSelectorViewComponent : ViewComponent
    {
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        //private readonly LocalizationSettings _localizationSettings;

        public LanguageSelectorViewComponent(ILanguageService languageService,
            IWorkContext workContext)
        {
            _languageService = languageService;
            _workContext = workContext;
            //_localizationSettings = localizationSettings;
        }

        private LanguageSelectorModel PrepareLanguageSelectorModel()
        {
            var availableLanguages = _languageService
                    .GetAll()
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
