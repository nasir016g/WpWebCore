using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Core.Caching
{
    /// <summary>
    /// Represents a manager for caching between HTTP requests (long term caching)
    /// </summary>
    public interface IStaticCacheManager : ICacheManager
    {
        T GetEasy<T>(string key, Func<T> acquire);
    }
}
