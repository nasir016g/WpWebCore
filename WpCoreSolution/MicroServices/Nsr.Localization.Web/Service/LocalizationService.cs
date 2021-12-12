using Nsr.Common.Core;
using Nsr.Localization.Web.Data;
using Nsr.Localization.Web.Data.Repositories;
using Nsr.RestClient.Models.Localization;
using System.Xml;

namespace Nsr.Localization.Web.Services
{


    public class LocalizationService : EntityService<LocaleStringResource>, ILocalizationService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// </remarks>
        private const string LOCALSTRINGRESOURCES_ALL_KEY = "Wp.lsr.all-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : resource key
        /// </remarks>
        private const string LOCALSTRINGRESOURCES_BY_RESOURCENAME_KEY = "Wp.lsr.{0}-{1}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string LOCALSTRINGRESOURCES_PATTERN_KEY = "Wp.lsr.";

        #endregion

        private readonly ILocalizationBaseRepository<LocaleStringResource> _lsrRepo;
        private readonly ILanguageService _languageService;
        //private readonly LocalizationSettings _localizationSettings;
       
        public LocalizationService(ILocalizationUnitOfWork unitOfWork, ILocalizationBaseRepository<LocaleStringResource> lsrRepo, ILanguageService languageService)
        : base(unitOfWork, lsrRepo)
        {
            this._lsrRepo = lsrRepo;
            this._languageService = languageService;
            //this._localizationSettings = localizationSettings;
           
        }

        public IEnumerable<LocaleStringResource> GetAll(int languageId, string sortOrder, string searchString, int page, int pageSize, out int total)
        {
            var resources = _lsrRepo.Table.Where(x => x.LanguageId == languageId);

             // IEnumerable.Contains method (.NET Framework) case-sensitive and returns all rows when an empty string is passed.
            // IQueryable.Contains method (Entity Framework provider for SQL Server) case-insensitive and returns zero rows when an empty string is passed. 
            if (!String.IsNullOrEmpty(searchString)) 
            {
                resources = resources.Where(x => x.ResourceName.ToUpper().Contains(searchString.ToUpper())
                    || x.ResourceValue.ToUpper().Contains(searchString.ToUpper()));
            }

            bool desc = false;
            string columnName = "ResourceName";

            if (sortOrder != null)
            {
                var sortValues = sortOrder.Split('_');

                columnName = sortValues[0];
                if (sortValues.Count() > 1)
                {
                    desc = (sortValues[1] == "desc");
                }
            }           

            switch (sortOrder)
            {
                case "Name_desc":
                    resources = resources.OrderByDescending(x => x.ResourceName);
                    break;
                case "Value":
                    resources = resources.OrderBy(x => x.ResourceValue);
                    //resources = resources.OrderBy("ResourceValue");
                    break;
                case "value_desc":
                    resources = resources.OrderByDescending(x => x.ResourceValue);
                    //resources = resources.OrderBy("ResourceValue", true);
                    break;
                default: // Name ascending
                    resources = resources.OrderBy(x => x.ResourceName);
                    break;
            }

            total = resources.Count();

            return resources
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        //public LocaleStringResource GetById(int id)
        //{
        //    return _lsrRepo.GetById(id);
        //}

        //public LocaleStringResource GetByName(string resourceName)
        //{
        //    using (var serviceScope = ServiceLocator.GetScope())
        //    {
        //        var workContext = serviceScope.ServiceProvider.GetService<IWorkContext>();
        //        return GetByName(resourceName, workContext.Current.WorkingLanguageId);
        //    }
        //}

        //public LocaleStringResource GetByName(string resourceName, int languageId)
        //{
        //    return _lsrRepo.Table.Where(x => x.ResourceName == resourceName && x.LanguageId == languageId).FirstOrDefault();
        //}

        public override void Insert(LocaleStringResource t)
        {
            if (t == null)
                throw new ArgumentNullException("localeStringResource");

            _lsrRepo.Add(t);
            _unitOfWork.Complete();
            

            //cache
            //_cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
        }

        public override void Update(LocaleStringResource t)
        {
            if (t == null)
                throw new ArgumentNullException("localeStringResource");

            _unitOfWork.Complete();
           

            //cache
            //_cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
        }

        public override void Delete(LocaleStringResource t)
        {
            _lsrRepo.Remove(t);
            _unitOfWork.Complete();

            //cache
            //_cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
        }

        public Dictionary<string, KeyValuePair<int, string>> GetAllResourceValues(int languageId)
        {
            string key = string.Format(LOCALSTRINGRESOURCES_ALL_KEY, languageId);
            //return _cacheManager.Get(key, () =>
            //    {
                    var query = from l in _lsrRepo.Table
                                orderby l.ResourceName
                                where l.LanguageId == languageId
                                select l;
                    var locales = query.ToList();
                    //format: <name, <id, value>>
                    var dictionary = new Dictionary<string, KeyValuePair<int, string>>();
                    foreach (var locale in locales)
                    {
                        var resourceName = locale.ResourceName.ToLowerInvariant();
                        if (!dictionary.ContainsKey(resourceName))
                            dictionary.Add(resourceName, new KeyValuePair<int, string>(locale.Id, locale.ResourceValue));
                    }
                    return dictionary;
                //});
        }

        public string GetResource(string resourceKey)
        {
            using (var serviceScope = ServiceLocator.GetScope())
            {
                var workContext = serviceScope.ServiceProvider.GetService<IWorkContext>();
                return GetResource(resourceKey, workContext.Current.WorkingLanguageId);
            }
               
        }

        public string GetResource(string resourceKey, int languageId, string defaultValue = "")
        {
            string result = string.Empty;
            if (resourceKey == null)
                resourceKey = string.Empty;
            resourceKey = resourceKey.Trim().ToLowerInvariant();

            //if (_localizationSettings.LoadAllLocaleRecordsOnStartup)
            //{
            //    //load all records (we know they are cached)
            //    var resources = GetAllResourceValues(languageId);
            //    if (resources.ContainsKey(resourceKey))
            //    {
            //        result = resources[resourceKey].Value;
            //    }
            //}
            //else
            //{
                //gradual loading
                string key = string.Format(LOCALSTRINGRESOURCES_BY_RESOURCENAME_KEY, languageId, resourceKey);
                //string lsr = _cacheManager.Get(key, () =>
                //{
                //    var query = from l in _lsrRepo.Table
                //                where l.ResourceName == resourceKey
                //                && l.LanguageId == languageId
                //                select l.ResourceValue;

                //    return query.FirstOrDefault();
                //});
                
                    var query = from l in _lsrRepo.Table
                                where l.ResourceName == resourceKey
                                && l.LanguageId == languageId
                                select l.ResourceValue;

                string lsr = query.FirstOrDefault();
                if (lsr != null)
                    result = lsr;               
          //  }

            return result;
        }

        public void ImportResourcesFromXml(Language language, string xml)
        {
            if (language == null)
                throw new ArgumentNullException("language");

            if (String.IsNullOrEmpty(xml))
                return;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            var nodes = xmlDoc.SelectNodes(@"//Language/LocaleResource");
            foreach (XmlNode node in nodes)
            {
                string name = node.Attributes["Name"].InnerText.Trim();
                string value = "";
                var valueNode = node.SelectSingleNode("Value");
                if (valueNode != null)
                    value = valueNode.InnerText;

                if (String.IsNullOrEmpty(name))
                    continue;

                var resource = language.LocaleStringResources.FirstOrDefault(x => x.ResourceName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                if (resource != null)
                    resource.ResourceValue = value;
                else
                {
                    language.LocaleStringResources.Add(
                        new LocaleStringResource()
                        {
                            ResourceName = name,
                            ResourceValue = value
                        });
                }
                _languageService.Update(language);
            }

            //clear cache
            //_cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
        }
    }
}
