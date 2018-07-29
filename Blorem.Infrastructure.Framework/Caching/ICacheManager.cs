using System;

namespace Blorem.Infrastructure.Framework.Caching
{
    
    public interface ICacheManager : IDisposable
    {
    
        T Get<T>(string key, Func<T> acquire, int cacheTime = 60);

        void Set(string key, object data, int cacheTime);

        bool IsSet(string key);

        void Remove(string key);

        void RemoveByPattern(string pattern);

        void Clear();
        
    }
}
