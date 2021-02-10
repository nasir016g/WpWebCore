using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nsr.Common.Core.Caching
{
    public class PerRequestCacheManager : ICacheManager
    {
        private readonly IHttpContextAccessor _context;

        public PerRequestCacheManager(IHttpContextAccessor context)
        {
            this._context = context;
        }

        protected IDictionary<object, object> GetItems()
        {
            if (_context != null)
                return _context.HttpContext.Items;

            return null;
        }

        public T Get<T>(string key)
        {
            var items = GetItems();
            if (items == null)
                return default(T);

            return (T)items[key];
        }

        public void Set(string key, object data, int cacheTime)
        {
            var items = GetItems();
            if (items == null)
                return;

            if (data != null)
            {
                if (items.ContainsKey(key))
                    items[key] = data;
                else
                    items.Add(key, data);
            }
        }

        public bool IsSet(string key)
        {
            var items = GetItems();
            if (items == null)
                return false;

            return (items[key] != null);
        }

        public void Remove(string key)
        {
            var items = GetItems();
            if (items == null)
                return;

            items.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            var items = GetItems();
            if (items == null)
                return;

            var enumerator = items.GetEnumerator();
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<String>();
            while (enumerator.MoveNext())
            {
                if (regex.IsMatch(enumerator.Current.Key.ToString()))
                {
                    keysToRemove.Add(enumerator.Current.Key.ToString());
                }
            }

            foreach (string key in keysToRemove)
            {
                items.Remove(key);
            }
        }

        public void Clear()
        {
            var items = GetItems();
            if (items == null)
                return;

            var enumerator = items.GetEnumerator();
            var keysToRemove = new List<String>();
            while (enumerator.MoveNext())
            {
                keysToRemove.Add(enumerator.Current.Key.ToString());
            }

            foreach (string key in keysToRemove)
            {
                items.Remove(key);
            }
        }
    }
}
