using System;
using System.Linq.Expressions;
using Wp.Core;
using Wp.Core.Configuration;
using Wp.Core.Domain.Configuration;

namespace Wp.Services.Configuration
{
    public partial interface ISettingService : IEntityService<Setting>
    {
        //Setting GetById(int settingId);

        void SetSetting<T>(string key, T value);
        T LoadSetting<T>() where T : ISettings, new();
        void SaveSetting<T>(T settings) where T : ISettings, new();
        void SaveSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) where T : ISettings, new();
        void DeleteSetting<T>() where T : ISettings, new();
        void DeleteSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) where T : ISettings, new();
        void ClearCache();
    }
}