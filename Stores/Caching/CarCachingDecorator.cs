using DecoratorAndCaching.Models;
using Microsoft.Extensions.Caching.Memory;

namespace DecoratorAndCaching.Stores.Caching;

public class CarCachingDecorator<T> : ICarStore where T : ICarStore
{
    private readonly IMemoryCache _memoryCache;
    // Inner aqui vai sera implementação do banco de dados
    private readonly T _inner;
    private readonly ILogger<CarCachingDecorator<T>> _logger;

    public CarCachingDecorator(IMemoryCache memoryCache, T inner, ILogger<CarCachingDecorator<T>> logger)
    {
        _memoryCache = memoryCache;
        _inner = inner;
        _logger = logger;
    }
    public CarDto Get(int id)
    {
        var key = $"Cars:{id}";
        var item = _memoryCache.Get<CarDto>(key);
        // se o item não estiver na memoria, vamos buscar ele do banco de dados e dps salvar ele na memoria
        if (item == null)
        {
            item = _inner.Get(id);
            _logger.LogTrace("Cache miss for {CacheKey}", key);
            // se ele estiver no banco, salve ele na memoria
            if (item != null)
            {
                item.FromMemory();
                _logger.LogTrace("Setting items for {CacheKey}", key);
                // ira ficar na memoria 1 minuto
                _memoryCache.Set(key, item, TimeSpan.FromMinutes(1));
            }
        }
        else
        {
            _logger.LogTrace("Cache hit for {CacheKey}", key);

        }
        return item;

    }

    public CarDto List()
    {

        var key = "Cars";
        var items = _memoryCache.Get<CarDto>(key);
        // se o item não estiver na memoria, vamos buscar ele do banco de dados e dps salvar ele na memoria
        if (items == null)
        {
            items = _inner.List();
            _logger.LogTrace("Cache miss for {CacheKey}", key);
            // se ele estiver no banco, salve ele na memoria
            if (items != null)
            {
                items.FromMemory();
                _logger.LogTrace("Setting items for {CacheKey}", key);
                // ira ficar na memoria 1 minuto
                _memoryCache.Set(key, items, TimeSpan.FromMinutes(1));
            }
        }
        else
        {
            _logger.LogTrace("Cache hit for {CacheKey}", key);

        }
        return items;

    }
}