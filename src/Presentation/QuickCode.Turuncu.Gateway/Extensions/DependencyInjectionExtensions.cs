using Yarp.ReverseProxy.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace QuickCode.Turuncu.Gateway.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IReverseProxyBuilder LoadFromMemory(this IReverseProxyBuilder builder)
        {
            builder.Services.AddSingleton<InMemoryConfigProvider>();

            builder.Services.AddSingleton<IHostedService>(ctx => ctx.GetRequiredService<InMemoryConfigProvider>());

            builder.Services.AddSingleton<IProxyConfigProvider>(ctx =>
                ctx.GetRequiredService<InMemoryConfigProvider>());

            return builder;
        }

        private static readonly object _cacheLock = new object();

        public static async Task<T> GetOrAddAsync<T>(this IMemoryCache cache, string key,
            Func<object[], Task<T>> valueFactory, TimeSpan expiration, params object[] args)
        {
            if (!cache.TryGetValue(key, out T cachedValue))
            {
                lock (_cacheLock)
                {
                    if (!cache.TryGetValue(key, out cachedValue))
                    {
                        cachedValue = valueFactory(args).Result; // Task sonuçlarını döndürmek için Result kullanılıyor
                        var cacheEntryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = expiration
                        };
                        cache.Set(key, cachedValue, cacheEntryOptions);
                    }
                }
            }

            return cachedValue;
        }
        
        public static async Task<T> GetOrAddAsync<T>(this IMemoryCache cache, string key,
            Func<object[], Task<T>> valueFactory, params object[] args)
        {
            if (!cache.TryGetValue(key, out T cachedValue))
            {
                lock (_cacheLock)
                {
                    if (!cache.TryGetValue(key, out cachedValue))
                    {
                        cachedValue = valueFactory(args).Result; // Task sonuçlarını döndürmek için Result kullanılıyor
                        var cacheEntryOptions = new MemoryCacheEntryOptions
                        {
                           
                        };
                        cache.Set(key, cachedValue, cacheEntryOptions);
                    }
                }
            }

            return cachedValue;
        }
    }
}
