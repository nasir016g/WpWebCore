using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Wp.Core;
using Wp.Core.Caching;
using Wp.Core.Common;
using Wp.Core.Configuration;

using Wp.Core.Domain.Configuration;
using Wp.Data;

namespace Wp.Services.Configuration
{ 
    public partial class SettingService : EntityService<Setting>, ISettingService
    {
        #region

        /// <summary>
        /// Key for cache per type
        /// </summary>
        private const string SETTINGS_BY_TYPE_KEY = "Wp.settings.type-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string SETTINGS_PATTERN_KEY = "Wp.settings.";

        #endregion

        private readonly IBaseRepository<Setting> _settingRepository;
        private readonly ICacheManager _cacheManager;      

        #region Ctor
      
        public SettingService(IUnitOfWork unitOfWork, IBaseRepository<Setting> settingRepository, ICacheManager cacheManager) : base(unitOfWork, settingRepository)
        {           
            this._settingRepository = settingRepository;
            this._cacheManager = cacheManager;
        }

        #endregion       

        #region Methods
              
        public override Setting GetById(int settingId)
        {
            if (settingId == 0)
                return null;

            return _settingRepository.GetById(settingId);
        }        

        #endregion

        #region Setting Type       

        public virtual void SetSetting<T>(string key, T value)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            key = key.Trim().ToLowerInvariant();
            string valueStr = CommonHelper.GetCustomTypeConverter(typeof(T)).ConvertToInvariantString(value);

            var setting = _settingRepository.Table.Where(x => x.Name == key).FirstOrDefault();           
            if (setting != null)
            {                
                setting.Value = valueStr;
                Update(setting);
            }
            else
            {
                //insert
                var newSetting = new Setting()
                {
                    Name = key,
                    Value = valueStr                  
                };
                Insert(newSetting);
            }
        }

        //public bool SettingExists<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector, int storeId = 0) where T : ISettings, new()
        //{
        //    throw new NotImplementedException();
        //}

        public virtual T LoadSetting<T>() where T : ISettings, new()
        {
            string cacheKey = string.Format(SETTINGS_BY_TYPE_KEY, typeof(T).Name);
            return _cacheManager.Get(cacheKey, () =>
            {
                var settings = Activator.CreateInstance<T>();

                foreach (var prop in typeof(T).GetProperties())
                {
                    // get properties we can read and write to
                    if (!prop.CanRead || !prop.CanWrite)
                        continue;

                    var key = typeof(T).Name + "." + prop.Name;

                    Setting entity = _settingRepository.Table.Where(x => x.Name == key).FirstOrDefault();

                    string setting = entity != null ? entity.Value : null;
                    if (setting == null)
                        continue;

                    if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                        continue;

                    if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting))
                        continue;

                    var value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting);

                    //set property
                    prop.SetValue(settings, value, null);
                }

                return settings;
            });           
        }

        public virtual void SaveSetting<T>(T settings) where T : ISettings, new()
        {
            string cacheKey = string.Format(SETTINGS_BY_TYPE_KEY, typeof(T).Name);

            foreach (var prop in typeof(T).GetProperties())
            {
                // get properties we can read and write to
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                if (!CommonHelper.GetCustomTypeConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                string key = typeof(T).Name + "." + prop.Name;
                //Duck typing is not supported in C#. That's why we're using dynamic type
                dynamic value = prop.GetValue(settings, null);
                if (value != null)
                    SetSetting(key, value);
                else
                    SetSetting(key, "");
            }

            ClearCache();
        }

        public virtual void SaveSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) where T : ISettings, new()
        {
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

            string key = typeof(T).Name + "." + propInfo.Name;
            //Duck typing is not supported in C#. That's why we're using dynamic type
            dynamic value = propInfo.GetValue(settings, null);
            if (value != null)
                SetSetting(key, value);
            else
                SetSetting(key, "");
        }

        public virtual void DeleteSetting<T>() where T : ISettings, new()
        {
            var settingsToDelete = new List<Setting>();
            var allSettings = GetAll();
            foreach (var prop in typeof(T).GetProperties())
            {
                string key = typeof(T).Name + "." + prop.Name;
                settingsToDelete.AddRange(allSettings.Where(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
            }

            foreach (var setting in settingsToDelete)
                Delete(setting);

            ClearCache();
        }

        public virtual void DeleteSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) where T : ISettings, new()
        {
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

            string key = typeof(T).Name + "." + propInfo.Name;
            key = key.Trim().ToLowerInvariant();
           
            //update
            var setting = _settingRepository.Table.Where(x => x.Name == key).FirstOrDefault();
            Delete(setting);            
        }

        public virtual void ClearCache()
        {
            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
        }
        #endregion
    }
}
