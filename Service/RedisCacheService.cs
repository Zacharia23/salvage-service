using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using SalvageCore.Interface;

namespace SalvageCore.Service;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache? _cache;

    public RedisCacheService(IDistributedCache? cache)
    {
        _cache = cache;
    }

    public T? GetData<T>(string key)
    {
        var data = _cache?.GetString(key);
        if (data is null) return default;

        return JsonSerializer.Deserialize<T>(data);
    }

    public void SetData<T>(string key, T data)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };

        _cache?.SetString(key, JsonSerializer.Serialize(data), options);
    }

    public async Task RemoveData(string key)
    {
        await _cache?.RemoveAsync(key)!;
    }
}