using Nsr.Common.Core.Localization.Models;
using Nsr.Common.Data;
using Nsr.Common.Data.Repositories;
using Nsr.Common.Services;
using System.Collections.Generic;
using System.Linq;

namespace Nsr.Common.Service.Localization
{

    public class LanguageService : EntityService<Language>, ILanguageService
    {
        #region Constants

        private const string LANGUAGES_ALL_KEY = "Wp.language.all.";

        #endregion

        private readonly ICommonBaseRepository<Language> _languageRepo;

        public LanguageService(ICommonUnitOfWork unitOfWork, ICommonBaseRepository<Language> languageRepo) : base(unitOfWork, languageRepo)
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
