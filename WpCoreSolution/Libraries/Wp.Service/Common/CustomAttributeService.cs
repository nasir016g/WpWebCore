using Nsr.Common.Core;
using Nsr.Common.Core.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using Wp.Core.Domain.Common;
using Wp.Services.Events;

namespace Wp.Services.Common
{
    public partial class CustomAttributeService : EntityService<CustomAttribute>, ICustomAttributeService
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

        private readonly IBaseRepository<CustomAttribute> _customAttributeRepository;
        private readonly IBaseRepository<CustomAttributeValue> _customAttributeValueRepository;
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
        public CustomAttributeService(IUnitOfWork unitOfWork, ICacheManager cacheManager,
            IBaseRepository<CustomAttribute> customAttributeRepository,
            IBaseRepository<CustomAttributeValue> customAttributeValueRepository,
            IEventPublisher eventPublisher
           ) : base(unitOfWork, customAttributeRepository, eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._customAttributeRepository = customAttributeRepository;
            this._customAttributeValueRepository = customAttributeValueRepository;
           
        }

        #endregion

        #region Methods

        public override IList<CustomAttribute> GetAll()
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

        public override CustomAttribute GetById(int id)
        {
            if (id == 0)
                return null;

            string key = string.Format(CUSTOMATTRIBUTES_BY_ID_KEY, id);
            return _cacheManager.Get(key, () => _customAttributeRepository.GetById(id));
        }


        public override void Insert(CustomAttribute entity)
        {
            if (entity == null)
                throw new ArgumentNullException("customAttribute");

            base.Insert(entity);

            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTEVALUES_PATTERN_KEY);
        }

        public override void Update(CustomAttribute entity)
        {
            if (entity == null)
                throw new ArgumentNullException("customAttribute");

            base.Update(entity);

            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTEVALUES_PATTERN_KEY);
        }

        public override void Delete(CustomAttribute entity)
        {
            if (entity == null)
                throw new ArgumentNullException("customAttributeValue");

            base.Delete(entity);

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

            _customAttributeValueRepository.Add(customAttributeValue);
            _unitOfWork.Complete();

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

            _unitOfWork.Complete();
            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CUSTOMATTRIBUTEVALUES_PATTERN_KEY);

        }

        #endregion
    }
}
