using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Wp.Core;
using Wp.Core.Caching;
using Wp.Core.Common;

using Wp.Core.Domain.Localization;
using Wp.Data;

namespace Wp.Services.Localization
{  
    public class LocalizedEntityService : ILocalizedEntityService
    {
        #region Constants

       /// <summary>
       /// Key for caching 
        /// {0} : language ID
        /// {1} : entity ID
        /// {2} : locale key group
        /// {3} : locale key
        /// </summary>        
        private const string LOCALIZEDPROPERTY_KEY = "Wp.localizedproperty.value-{0}-{1}-{2}-{3}";

        /// <summary>
        /// Key for caching
        /// {0} : locale key group
        /// {1} : locale key
        /// {2} : locale value
        /// </summary>
        private const string LOCALIZEDPROPERTY_ENTITYID_KEY = "Wp.localizedproperty.entityid-{0}-{1}-{2}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string LOCALIZEDPROPERTY_PATTERN_KEY = "Wp.localizedproperty.";

        #endregion

        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<LocalizedProperty> _localizedPropertyRepository;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        public LocalizedEntityService(ICacheManager cacheManager, IUnitOfWork unitOfWork, 
            IBaseRepository<LocalizedProperty> localizedPropertyRepository)
        {
            this._cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            this._localizedPropertyRepository = localizedPropertyRepository;           
        }

        #endregion

        public LocalizedProperty GetLocalizedPropertyById(int localizedPropertyId)
        {
            if (localizedPropertyId == 0)
                return null;

            return _localizedPropertyRepository.GetById(localizedPropertyId);        
        }

        public int GetEntityIdByLocalizedValue(string localeKeyGroup, string localeKey, string localeValue)
        {
            string key = string.Format(LOCALIZEDPROPERTY_ENTITYID_KEY, localeKeyGroup, localeKey, localeValue);
            return _cacheManager.Get(key, () =>
            {
                var query = from lp in _localizedPropertyRepository.Table
                            where lp.LocaleKeyGroup == localeKeyGroup &&
                            lp.LocaleKey == localeKey &&
                            lp.LocaleValue == localeValue
                            select lp.EntityId;
                var result = query.FirstOrDefault();
                return result;
            });
        }

        public virtual string GetLocalizedValue(int languageId, int entityId, string localeKeyGroup, string localeKey)
        {
            string key = string.Format(LOCALIZEDPROPERTY_KEY, languageId, entityId, localeKeyGroup, localeKey);
            return _cacheManager.Get(key, () =>
            {
                var query = from lp in _localizedPropertyRepository.Table
                            where lp.LanguageId == languageId &&
                            lp.EntityId == entityId &&
                            lp.LocaleKeyGroup == localeKeyGroup &&
                            lp.LocaleKey == localeKey
                            select lp.LocaleValue;
                var localeValue = query.FirstOrDefault();
                //little hack here. nulls aren't cacheable so set it to ""
                if (localeValue == null)
                    localeValue = "";
                return localeValue;
            });
        }

        public IList<LocalizedProperty> GetLocalizedProperties(int entityId, string localeKeyGroup)
        {
            if (entityId == 0 || string.IsNullOrEmpty(localeKeyGroup))
                return new List<LocalizedProperty>();

            var query = from lp in _localizedPropertyRepository.Table
                        orderby lp.EntityId
                        where lp.EntityId == entityId &&
                              lp.LocaleKeyGroup == localeKeyGroup
                        select lp;
            var props = query.ToList();
            return props;
        }

        public void SaveLocalizedValue<T>(T entity, Expression<Func<T, string>> keySelector, string localeValue, int languageId)
            where T : IEntity, ILocalizedEntity
        {
            SaveLocalizedValue<T, string>(entity, keySelector, localeValue, languageId);
        }

        public virtual void SaveLocalizedValue<T, TPropType>(T entity, Expression<Func<T, TPropType>> keySelector, TPropType localeValue, int languageId)
            where T : IEntity, ILocalizedEntity
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (languageId == 0)
                throw new ArgumentOutOfRangeException("languageId", "Language ID should not be 0");

            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }

            string localeKeyGroup = typeof(T).Name;
            string localeKey = propInfo.Name;

            var props = GetLocalizedProperties(entity.Id, localeKeyGroup);
            var prop = props.FirstOrDefault(lp => lp.LanguageId == languageId &&
                lp.LocaleKey.Equals(localeKey, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            string localeValueStr = CommonHelper.To<string>(localeValue);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(localeValueStr))
                {
                    //delete
                    DeleteLocalizedProperty(prop);
                }
                else
                {
                    //update
                    prop.LocaleValue = localeValueStr;
                    UpdateLocalizedProperty(prop);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(localeValueStr))
                {
                    //insert
                    prop = new LocalizedProperty()
                    {
                        EntityId = entity.Id,
                        LanguageId = languageId,
                        LocaleKey = localeKey,
                        LocaleKeyGroup = localeKeyGroup,
                        LocaleValue = localeValueStr
                    };
                    InsertLocalizedProperty(prop);
                }
            }
        }

        public void DeleteLocalizedProperty(LocalizedProperty localizedProperty)
        {
            if (localizedProperty == null)
                throw new ArgumentNullException("localizedProperty");

            _localizedPropertyRepository.Remove(localizedProperty);
            _unitOfWork.Complete();

            //cache            
            _cacheManager.RemoveByPattern(LOCALIZEDPROPERTY_PATTERN_KEY);            
           
        }

        public void InsertLocalizedProperty(LocalizedProperty localizedProperty)
        {
            if (localizedProperty == null)
                throw new ArgumentNullException("localizedProperty");

            _localizedPropertyRepository.Add(localizedProperty);
            

            //cache
            _cacheManager.RemoveByPattern(LOCALIZEDPROPERTY_PATTERN_KEY);
        }

        public void UpdateLocalizedProperty(LocalizedProperty localizedProperty)
        {
            if (localizedProperty == null)
                throw new ArgumentNullException("localizedProperty");

            //_localizedPropertyRepository.(localizedProperty);
            _unitOfWork.Complete();

            //cache
            _cacheManager.RemoveByPattern(LOCALIZEDPROPERTY_PATTERN_KEY);
        }
    }
}
