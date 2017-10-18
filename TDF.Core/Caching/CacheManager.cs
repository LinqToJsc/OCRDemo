namespace TDF.Core.Caching
{
    public class CacheManager
    {

        public static ICacheProvider Cache => Ioc.Ioc.Resolve<ICacheProvider>();

        public static T Get<T>(string key)
        {
            return Cache.Get<T>(key);
        }

        public static void Set(string key, object data, int cacheTime)
        {
            Cache.Set(key, data, cacheTime);
        }

        public static bool IsSet(string key)
        {
            return Cache.IsSet(key);
        }

        public static void Remove(string key)
        {
            Cache.Remove(key);
        }

        public static void RemoveByPattern(string pattern)
        {
            Cache.RemoveByPattern(pattern);
        }

        public static void Clear()
        {
            Cache.Clear();
        }
    }
}
