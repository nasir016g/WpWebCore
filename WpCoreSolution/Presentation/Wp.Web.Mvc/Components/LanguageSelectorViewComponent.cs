//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Wp.Core;
//using Wp.Core.Domain.Localization;
//using Wp.Services.Localization;
//using Wp.Web.Mvc.Models;

//namespace Wp.Web.Mvc.Components
//{
//    public class LanguageSelectorViewComponent : ViewComponent
//    {
//        private readonly ILanguageService _languageService;
//        private readonly IWorkContext _workContext;
//        private readonly LocalizationSettings _localizationSettings;

//        public LanguageSelectorViewComponent(ILanguageService languageService,
//            IWorkContext workContext,
//            LocalizationSettings localizationSettings)
//        {
//            _languageService = languageService;
//            _workContext = workContext;
//            _localizationSettings = localizationSettings;
//        }

//        private LanguageSelectorModel PrepareLanguageSelectorModel()
//        {
//            var availableLanguages = _languageService
//                    .GetAll()
//                    .Select(x => new LanguageModel()
//                    {
//                        Id = x.Id,
//                        Name = x.Name,
//                        FlagImageFileName = x.FlagImageFileName,
//                    })
//                    .ToList();

//            var model = new LanguageSelectorModel();
//            if (availableLanguages.Count() > 0)
//            {
//                model.CurrentLanguageId = _workContext.Current.WorkingLanguage.Id;
//                model.AvailableLanguages = availableLanguages;
//                model.UseImages = _localizationSettings.UseImagesForLanguageSelection;
//            }

//            return model;
//        }
    
//        public IViewComponentResult Invoke()
//        {
//            var model = PrepareLanguageSelectorModel();
//            return View(model);
//        }
//    }
//}
