using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wp.Core;
using Wp.Core.Caching;
using Wp.Core.Common;
using Wp.Core.Domain.Common;
using Wp.Data;

namespace Wp.Services.Common
{
    public partial class GenericAttributeService : IGenericAttributeService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : key group
        /// </remarks>
        private const string GENERICATTRIBUTE_KEY = "Wp.genericattribute.{0}-{1}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string GENERICATTRIBUTE_PATTERN_KEY = "Wp.genericattribute.";
        #endregion

        #region Fields

        private readonly IEntityBaseRepository<GenericAttribute> _genericAttributeRepository;
        private readonly ICacheManager _cacheManager;
       

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="genericAttributeRepository">Generic attribute repository</param>
        /// <param name="eventPublisher">Event published</param>
        public GenericAttributeService(ICacheManager cacheManager,
            IEntityBaseRepository<GenericAttribute> genericAttributeRepository)            
        {
            _cacheManager = cacheManager;
            _genericAttributeRepository = genericAttributeRepository;
        }

        #endregion

        #region Methods
      
        public virtual void DeleteAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            _genericAttributeRepository.Delete(attribute);

            //cache
            _cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);
        }

        public virtual void DeleteAttributes(IList<GenericAttribute> attributes)
        {
            if (attributes == null)
                throw new ArgumentNullException("attributes");

            //cache
            _cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);

            //event notification
            foreach (var attribute in attributes)
            {
                _genericAttributeRepository.Delete(attribute);
            }
        }
       
        public virtual GenericAttribute GetAttributeById(int attributeId)
        {
            if (attributeId == 0)
                return null;

            return _genericAttributeRepository.GetById(attributeId);
        }

        public virtual void InsertAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            _genericAttributeRepository.Save(attribute);

            //cache
            _cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);          
        }
      
        public virtual void UpdateAttribute(GenericAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");

            _genericAttributeRepository.Save(attribute);

            //cache
            _cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);
            
        }

        /// <summary>
        /// Get attributes
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="keyGroup">Key group</param>
        /// <returns>Get attributes</returns>
        public virtual IList<GenericAttribute> GetAttributesForEntity(int entityId, string keyGroup)
        {
            string key = string.Format(GENERICATTRIBUTE_KEY, entityId, keyGroup);
            return _cacheManager.Get(key, () =>
            {
                var query = from ga in _genericAttributeRepository.Table
                            where ga.EntityId == entityId &&
                            ga.KeyGroup == keyGroup
                            select ga;
                var attributes = query.ToList();
                return attributes;
            });
        }

        /// <summary>
        /// Save attribute value
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="storeId">Store identifier; pass 0 if this attribute will be available for all stores</param>
        public virtual void SaveAttribute<TPropType>(Entity entity, string key, TPropType value)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            if (key == null)
                throw new ArgumentNullException("key");

            string keyGroup = entity.GetUnproxiedEntityType().Name;

            var props = GetAttributesForEntity(entity.Id, keyGroup)               
                .ToList();
            var prop = props.FirstOrDefault(ga =>
                ga.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            var valueStr = CommonHelper.To<string>(value);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(valueStr))
                {
                    //delete
                    DeleteAttribute(prop);
                }
                else
                {
                    //update
                    prop.Value = valueStr;
                    UpdateAttribute(prop);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(valueStr))
                {
                    //insert
                    prop = new GenericAttribute
                    {
                        EntityId = entity.Id,
                        Key = key,
                        KeyGroup = keyGroup,
                        Value = valueStr,
                       

                    };
                    InsertAttribute(prop);
                }
            }
        }

        #endregion
    }
}
