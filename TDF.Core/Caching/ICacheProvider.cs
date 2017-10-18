using System;

namespace TDF.Core.Caching
{
    /// <summary>
    /// 实现该接口的类型是能够为应用程序提供缓存机制的类型。
    /// </summary>
    public interface ICacheProvider : IDisposable
    {
        T Get<T>(string key);

        void Set(string key, object data, int cacheTime);

        bool IsSet(string key);

        void Remove(string key);

        void RemoveByPattern(string pattern);

        void Clear();
    }
}
