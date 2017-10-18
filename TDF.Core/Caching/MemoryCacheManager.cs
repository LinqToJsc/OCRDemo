using System;
using System.Linq;
using System.Runtime.Caching;

namespace TDF.Core.Caching
{
    public partial class MemoryCacheManager : ICacheProvider
    {
        protected ObjectCache Cache => MemoryCache.Default;

        public virtual T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime)
            };
            if (IsSet(key))
            {
                Remove(key);
            }
            Cache.Add(new CacheItem(key, data), policy);
        }

        public virtual bool IsSet(string key)
        {
            return Cache.Contains(key);
        }

        public virtual void Remove(string key)
        {
            Cache.Remove(key);
        }

        public virtual void RemoveByPattern(string pattern)
        {
            this.RemoveByPattern(pattern, Cache.Select(x => x.Key));
        }

        public virtual void Clear()
        {
            foreach (var item in Cache)
            {
                Remove(item.Key);
            }
        }

        public virtual void Dispose()
        {
        }
    }
}
