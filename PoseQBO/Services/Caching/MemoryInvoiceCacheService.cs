using Microsoft.Extensions.Caching.Distributed;
using PoseQBO.Services.Caching.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Services.Caching
{
    public class MemoryInvoiceCacheService : IInvoiceCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly string KeyPrefix = "Invoices";
        private string _cacheKey;

        public MemoryInvoiceCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public string CacheKey { get => _cacheKey; private set => _cacheKey = value; }

        public async Task<byte[]> GetInvoiceItemsAsync(string cacheKey)
        {
            return await _cache.GetAsync(cacheKey);
        }

        public async Task<byte[]> GetInvoiceItemsAsync(string startDate, string endDate)
        {
            byte[] cachedItems = null;
            _cacheKey = !string.IsNullOrEmpty(_cacheKey) ? _cacheKey : $"{KeyPrefix}_{startDate}-{endDate}";
            cachedItems = await _cache.GetAsync(_cacheKey);
            return cachedItems;
        }

        public async Task SetInvoiceItemsAsync(string startDate, string endDate, byte[] items)
        {
            _cacheKey = !string.IsNullOrEmpty(_cacheKey) ? _cacheKey : $"{KeyPrefix}_{startDate}-{endDate}";
            await _cache.SetAsync(_cacheKey, items, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(2) });
        }
    }
}
