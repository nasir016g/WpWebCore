using Nsr.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nsr.Common.Service.Configuration
{
    public partial interface ISettingService : IEntityService<Setting>
    {

        void SetSetting<T>(string key, T value);
        T LoadSetting<T>() where T : ISettings, new();
        void SaveSetting<T>(T settings) where T : ISettings, new();
        void SaveSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) where T : ISettings, new();
        void DeleteSetting<T>() where T : ISettings, new();
        void DeleteSetting<T, TPropType>(T settings, Expression<Func<T, TPropType>> keySelector) where T : ISettings, new();
        void ClearCache();
    }
}
