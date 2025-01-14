using Microsoft.Extensions.Caching.Distributed;
using PosTech.Contacts.ApplicationCore.Services;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PosTech.Contacts.Infrastructure.Services
{
    public class CacheService(IDistributedCache distributedCache) : ICacheService
    {
        private readonly IDistributedCache _distributedCache = distributedCache;
        private JsonSerializerOptions _jsonSerializerOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        public async Task SetAsync<T>(string key, T value, TimeSpan expiresIn)
        {
            var serializedObject = JsonSerializer.Serialize(value, _jsonSerializerOptions);
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiresIn };
            var bytes = Encoding.UTF8.GetBytes(serializedObject);

            await _distributedCache.SetAsync(key, bytes, options);
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            value = default;
            var cachedValue = _distributedCache.Get(key);

            if (cachedValue is null) return false;

            value = JsonSerializer.Deserialize<T>(cachedValue, _jsonSerializerOptions);
        
            return true;
        }
    }
}
