using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TDF.Core.Caching
{
    public static class CacheExtensions
    {
        public static T Get<T>(this ICacheProvider cacheProvider, string key, Func<T> acquire)
        {
            return Get(cacheProvider, key, 60, acquire);
        }

        public static T Get<T>(this ICacheProvider cacheProvider, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheProvider.IsSet(key))
            {
                return cacheProvider.Get<T>(key);
            }
            var result = acquire.Invoke();
            if (cacheTime > 0)
            {
                cacheProvider.Set(key, result, cacheTime);
            }
            return result;
        }

        public static void RemoveByPattern(this ICacheProvider cacheProvider, string pattern, IEnumerable<string> keys)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (var key in keys.Where(p => regex.IsMatch(p)).ToList())
            {
                cacheProvider.Remove(key);
            }
        }
    }
}
