using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Blorem.Presentation.Main.Caching
{

    public partial class RedisCacheManager : ICacheManager
    {
        
        private readonly IRedisConnectionWrapper _connectionWrapper;
        private readonly IDatabase _db;

        public RedisCacheManager(IRedisConnectionWrapper connectionWrapper)
        {
            this._connectionWrapper = connectionWrapper;
            this._db = _connectionWrapper.GetDatabase();
        }

        protected virtual async Task<T> GetAsync<T>(string key)
        {
        
            var serializedItem = await _db.StringGetAsync(key);
            if (!serializedItem.HasValue)
                return default(T);

            var item = JsonConvert.DeserializeObject<T>(serializedItem);
            if (item == null)
                return default(T);

            return item;
        }

        protected virtual async Task SetAsync(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var expiresIn = TimeSpan.FromMinutes(cacheTime);

            var serializedItem = JsonConvert.SerializeObject(data);

            await _db.StringSetAsync(key, serializedItem, expiresIn);
        }

        protected virtual async Task<bool> IsSetAsync(string key)
        {
            return await _db.KeyExistsAsync(key);
        }

        protected virtual async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);         
        }

        protected virtual async Task RemoveByPatternAsync(string pattern)
        {
            foreach (var endPoint in _connectionWrapper.GetEndPoints())
            {
                var server = _connectionWrapper.GetServer(endPoint);
                var keys = server.Keys(database: _db.Database, pattern: $"*{pattern}*");
            
                await _db.KeyDeleteAsync(keys.ToArray());
            }
        }

        protected virtual async Task ClearAsync()
        {

            foreach (var endPoint in _connectionWrapper.GetEndPoints())
            {
                var server = _connectionWrapper.GetServer(endPoint);
                var keys = server.Keys(database: _db.Database);
                
                await _db.KeyDeleteAsync(keys.ToArray());
            }
        }

        public virtual T Get<T>(string key, Func<T> acquire, int cacheTime = 60)
        {
            if (this.IsSetAsync(key).Result)
                return this.GetAsync<T>(key).Result;

            var result = acquire();

            if (cacheTime > 0)
                this.SetAsync(key, result, cacheTime).Wait();

            return result;
        }

        public virtual async void Set(string key, object data, int cacheTime)
        {
            await this.SetAsync(key, data, cacheTime);
        }

        public virtual bool IsSet(string key)
        {
            return this.IsSetAsync(key).Result;
        }

        public virtual async void Remove(string key)
        {
            await this.RemoveAsync(key);
        }

        public virtual async void RemoveByPattern(string pattern)
        {
            await this.RemoveByPatternAsync(pattern);
        }

        public virtual async void Clear()
        {
            await this.ClearAsync();
        }

        public virtual void Dispose()
        {
            //if (_connectionWrapper != null)
            //    _connectionWrapper.Dispose();
        }
        
    }
}