using Nsr.Localization.Web.Data;
using Nsr.Localization.Web.Data.Repositories;
using Nsr.Localization.Web.Services;
using Nsr.RestClient.Models.Localization;

namespace Nsr.Localization.Web.Service
{
    public interface ILocalizationInstallationService
    {
        void InstallData();
    }
    public class LocalizationInstallationService : ILocalizationInstallationService
    {
        private readonly ILocalizationBaseRepository<Language> _languageRepo;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizationUnitOfWork _localizationUnitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public LocalizationInstallationService(ILocalizationBaseRepository<Language> languageRepo, 
            ILocalizationService localizationService, 
            ILocalizationUnitOfWork localizationUnitOfWork,
             IWebHostEnvironment hostEnvironment)
        {
            _languageRepo = languageRepo;
            _localizationService = localizationService;
            _localizationUnitOfWork = localizationUnitOfWork;
            _hostEnvironment = hostEnvironment;
        }       

        private void InstallLanguages()
        {
            if (_languageRepo.Table.Count() == 0)
            {
                var languages = new List<Language>()
                {
                    new Language { Name = "English", LanguageCulture = "en-Us", UniqueSeoCode = "en", FlagImageFileName = "us.png", Published = true },
                    new Language { Name = "Nederlands", LanguageCulture = "nl-NL", UniqueSeoCode = "nl", FlagImageFileName = "nl.png", Published = true }
                };

                languages.ForEach(l => _languageRepo.Add(l));
                _localizationUnitOfWork.Complete();

            }

            InstallLocaleResources();
        }

        private void InstallLocaleResources()
        {
            if (_localizationService.GetAll().Count > 0) return;
            var contentRootPath = _hostEnvironment.ContentRootPath;
            foreach (var language in _languageRepo.Table.ToList())
            {
                foreach (var filePath in System.IO.Directory.EnumerateFiles(Path.Combine(contentRootPath, "Files/Localization/"), string.Format("*.{0}.res.xml", language.UniqueSeoCode), SearchOption.TopDirectoryOnly))
                {
                    // 
                    string xmlText = File.ReadAllText(filePath);
                    //var localizationService = ServiceLocator.Instance.GetService<ILocalizationService>();
                    _localizationService.ImportResourcesFromXml(language, xmlText);
                }
            }

            _localizationUnitOfWork.Complete();
        }

        public void InstallData()
        {
            InstallLanguages();
        }
    }
}
