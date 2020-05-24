using System.Collections.Generic;
using System.Linq;
using Wp.Core;
using Wp.Core.Caching;
using Wp.Core.Domain.Localization;
using Wp.Data;

namespace Wp.Services.Localization
{
   
    public class LanguageService : EntityService<Language>, ILanguageService
    {
        #region Constants

        private const string LANGUAGES_ALL_KEY = "Wp.language.all.";

        #endregion

        private readonly IBaseRepository<Language> _languageRepo;
        private readonly ICacheManager _cacheManager;

        public LanguageService(IUnitOfWork unitOfWork, IBaseRepository<Language> languageRepo, ICacheManager cacheManager) : base(unitOfWork, languageRepo)
        {
            this._languageRepo = languageRepo;
            this._cacheManager = cacheManager;
        }

        public override IList<Language> GetAll()
        {
            string key = string.Format(LANGUAGES_ALL_KEY);
            return _cacheManager.Get(key, () =>
                {
                   return _languageRepo.Table.ToList();
                });
        }

               
    }
}
