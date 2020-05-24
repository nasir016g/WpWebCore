using System;
using System.Collections.Generic;
using System.Linq;
using Wp.Core.Caching;
using Wp.Core.Domain.Common;
using Wp.Data;

namespace Wp.Services.Common
{
    public partial class CustomAttributeService : ICustomAttributeService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        private const string CUSTOMATTRIBUTES_ALL_KEY = "Wp.customattribute.all";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : custom attribute ID
        /// </remarks>
        private const string CUSTOMATTRIBUTES_BY_ID_KEY = "Wp.customattribute.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : custom attribute ID
        /// </remarks>
        private const string CUSTOMATTRIBUTEVALUES_ALL_KEY = "Wp.customattributevalue.all-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : custom attribute value ID
        /// </remarks>
        private const string CUSTOMATTRIBUTEVALUES_BY_ID_KEY = "Wp.customattributevalue.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CUSTOMATTRIBUTES_PATTERN_KEY = "Wp.customattribute.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CUSTOMATTRIBUTEVALUES_PATTERN_KEY = "Wp.customattributevalue.";
        #endregion

        #region Fields

        private readonly IEntityBaseRepository<CustomAttribute> _customAttributeRepository;
        private readonly IEntityBaseRepository<CustomAttributeValue> _customAttributeValueRepository;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="customAttributeRepository">Custom attribute repository</param>
        /// <param name="customAttributeValueRepository">Custom attribute value repository</param>
        /// <param name="eventPublisher">Event published</param>
        public CustomAttributeService(ICacheManager cacheManager,
            IEntityBaseRepository<CustomAttribute> customAttributeRepository,
            IEntityBaseRepository<CustomAttributeValue> customAttributeValueRepository
           )
        {
            this._cacheManager = cacheManager;
            this._customAttributeRepository = customAttributeRepository;
            this._customAttributeValueRepository = customAttributeValueRepository;
           
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes an custom attribute
        /// </summary>
        /// <param name="customAttribute">Custom attribute</param>
        public virtual void DeleteCustomAttribute(CustomAttribute customAttribute)
        {
            if (customAttribute == null)
                throw new ArgumentNullException("customAttribute");

            _customAttributeRepository.Delete(customAttribute);

            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTEVALUES_PATTERN_KEY);

         
        }

        /// <summary>
        /// Gets all custom attributes
        /// </summary>
        /// <returns>Custom attributes</returns>
        public virtual IList<CustomAttribute> GetAllCustomAttributes()
        {
            string key = CUSTOMATTRIBUTES_ALL_KEY;
            return _cacheManager.Get(key, () =>
            {
                var query = from aa in _customAttributeRepository.Table
                            orderby aa.DisplayOrder
                            select aa;
                return query.ToList();
            });
        }

        /// <summary>
        /// Gets an custom attribute 
        /// </summary>
        /// <param name="customAttributeId">Custom attribute identifier</param>
        /// <returns>Custom attribute</returns>
        public virtual CustomAttribute GetCustomAttributeById(int customAttributeId)
        {
            if (customAttributeId == 0)
                return null;

            string key = string.Format(CUSTOMATTRIBUTES_BY_ID_KEY, customAttributeId);
            return _cacheManager.Get(key, () => _customAttributeRepository.GetById(customAttributeId));
        }

        /// <summary>
        /// Inserts an custom attribute
        /// </summary>
        /// <param name="customAttribute">Custom attribute</param>
        public virtual void InsertCustomAttribute(CustomAttribute customAttribute)
        {
            if (customAttribute == null)
                throw new ArgumentNullException("customAttribute");

            _customAttributeRepository.Save(customAttribute);

            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTEVALUES_PATTERN_KEY);

        }

        /// <summary>
        /// Updates the custom attribute
        /// </summary>
        /// <param name="customAttribute">Custom attribute</param>
        public virtual void UpdateCustomAttribute(CustomAttribute customAttribute)
        {
            if (customAttribute == null)
                throw new ArgumentNullException("customAttribute");

            _customAttributeRepository.Save(customAttribute);

            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTEVALUES_PATTERN_KEY);

        }

        /// <summary>
        /// Deletes an custom attribute value
        /// </summary>
        /// <param name="customAttributeValue">Custom attribute value</param>
        public virtual void DeleteCustomAttributeValue(CustomAttributeValue customAttributeValue)
        {
            if (customAttributeValue == null)
                throw new ArgumentNullException("customAttributeValue");

            _customAttributeValueRepository.Delete(customAttributeValue);

            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTEVALUES_PATTERN_KEY);

        }

        /// <summary>
        /// Gets custom attribute values by custom attribute identifier
        /// </summary>
        /// <param name="customAttributeId">The custom attribute identifier</param>
        /// <returns>Custom attribute values</returns>
        public virtual IList<CustomAttributeValue> GetCustomAttributeValues(int customAttributeId)
        {
            string key = string.Format(CUSTOMATTRIBUTEVALUES_ALL_KEY, customAttributeId);
            return _cacheManager.Get(key, () =>
            {
                var query = from aav in _customAttributeValueRepository.Table
                            orderby aav.DisplayOrder
                            where aav.CustomAttributeId == customAttributeId
                            select aav;
                var customAttributeValues = query.ToList();
                return customAttributeValues;
            });
        }

        /// <summary>
        /// Gets an custom attribute value
        /// </summary>
        /// <param name="customAttributeValueId">Custom attribute value identifier</param>
        /// <returns>Custom attribute value</returns>
        public virtual CustomAttributeValue GetCustomAttributeValueById(int customAttributeValueId)
        {
            if (customAttributeValueId == 0)
                return null;

            string key = string.Format(CUSTOMATTRIBUTEVALUES_BY_ID_KEY, customAttributeValueId);
            return _cacheManager.Get(key, () => _customAttributeValueRepository.GetById(customAttributeValueId));
        }

        /// <summary>
        /// Inserts an custom attribute value
        /// </summary>
        /// <param name="customAttributeValue">Custom attribute value</param>
        public virtual void InsertCustomAttributeValue(CustomAttributeValue customAttributeValue)
        {
            if (customAttributeValue == null)
                throw new ArgumentNullException("customAttributeValue");

            _customAttributeValueRepository.Save(customAttributeValue);

            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTEVALUES_PATTERN_KEY);

        }

        /// <summary>
        /// Updates the custom attribute value
        /// </summary>
        /// <param name="customAttributeValue">Custom attribute value</param>
        public virtual void UpdateCustomAttributeValue(CustomAttributeValue customAttributeValue)
        {
            if (customAttributeValue == null)
                throw new ArgumentNullException("customAttributeValue");

            _customAttributeValueRepository.Save(customAttributeValue);

            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTEVALUES_PATTERN_KEY);

        }

        #endregion
    }
}
