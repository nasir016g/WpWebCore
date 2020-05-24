using EasyCaching.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wp.Core.Caching
{
    public class EasyMemoryCacheManager : IStaticCacheManager
    {
        private readonly IEasyCachingProvider provider;

        public EasyMemoryCacheManager(IEasyCachingProvider easyCachingProvider)
        {
            this.provider = easyCachingProvider;
        }
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public T GetEasy<T>(string key, Func<T> acquire)
        {
            return provider.Get(key, acquire, TimeSpan.FromMinutes(60)).Value;
        }

        public bool IsSet(string key)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveByPattern(string pattern)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object data, int cacheTime)
        {
            throw new NotImplementedException();
        }
    }
}
