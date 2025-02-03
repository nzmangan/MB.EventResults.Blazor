using Microsoft.Extensions.Caching.Memory;

namespace MB.EventResults.Blazor.Server;

public class CacheService(ILogger<CacheService> _Logger, IMemoryCache _MemoryCache) : ICacheService {
  private readonly TimeSpan _CacheExpiration = TimeSpan.FromMinutes(5);
  private readonly List<string> _Keys = [];

  public async Task<T> Get<T>(string key, Func<Task<T>> builder) {
    if (_MemoryCache.TryGetValue<T>(key, out T record)) {
      return record;
    }

    var data = await builder();

    _MemoryCache.Set(key, data, new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = _CacheExpiration });
    _Keys.Add(key);

    _Logger.LogInformation($"Cache miss {key}");

    return data;
  }

  public Task Clear() {
    foreach (string cacheKey in _Keys) {
      _MemoryCache.Remove(cacheKey);
    }
    _Keys.Clear();
    return Task.CompletedTask;
  }
}