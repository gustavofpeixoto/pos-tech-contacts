using Microsoft.Extensions.Caching.Distributed;
using PosTech.Contacts.ApplicationCore.Serialization;
using PosTech.Contacts.ApplicationCore.Services;
using System.Text;

namespace PosTech.Contacts.Infrastructure.Services
{
    public class CacheService(IDistributedCache distributedCache) : ICacheService
    {
        private readonly IDistributedCache _distributedCache = distributedCache;

        public async Task SetAsync<T>(string key, T value, TimeSpan expiresIn)
        {
            var serializedObject = JsonSerializerHelper.Serialize(value);
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiresIn };
            var bytes = Encoding.UTF8.GetBytes(serializedObject);

            await _distributedCache.SetAsync(key, bytes, options);
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            value = default;
            var cachedValue = _distributedCache.GetString(key);

            if (cachedValue is null) return false;

            value = JsonSerializerHelper.Deserialize<T>(cachedValue);
        
            return true;
        }
    }
}
