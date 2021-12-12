using Nsr.Localization.Web.Data;
using Nsr.Localization.Web.Data.Repositories;
using Nsr.RestClient.Models.Localization;

namespace Nsr.Localization.Web.Services
{

    public class LanguageService : EntityService<Language>, ILanguageService
    {
        #region Constants

        private const string LANGUAGES_ALL_KEY = "Wp.language.all.";

        #endregion

        private readonly ILocalizationBaseRepository<Language> _languageRepo;

        public LanguageService(ILocalizationUnitOfWork unitOfWork, ILocalizationBaseRepository<Language> languageRepo) : base(unitOfWork, languageRepo)
        {
            this._languageRepo = languageRepo;
        }

        public override IList<Language> GetAll()
        {
            //string key = string.Format(LANGUAGES_ALL_KEY);
            //return _cacheManager.Get(key, () =>
            //    {
            //       return _languageRepo.Table.ToList();
            //    });
            return _languageRepo.Table.ToList();
        }

               
    }
}
