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
            SetCacheKey(startDate, endDate);
            return await GetCachedInvoicesAsync();
        }

        public async Task<byte[]> GetInvoiceItemsAsync(string companyName, string startDate, string endDate)
        {
            SetCacheKey(companyName, startDate, endDate);
            return await GetCachedInvoicesAsync();
        }


        public async Task SetInvoiceItemsAsync(string startDate, string endDate, byte[] items)
        {
            SetCacheKey(startDate, endDate);
            await SetCachedInvoicesAsync(items);
        }

        public async Task SetInvoiceItemsAsync(string companyName, string startDate, string endDate, byte[] items)
        {
            SetCacheKey(companyName, startDate, endDate);
            await SetCachedInvoicesAsync(items);
        }

        private async Task<byte[]> GetCachedInvoicesAsync()
        {
            return await _cache.GetAsync(_cacheKey);
        }

        private async Task SetCachedInvoicesAsync(byte[] items)
        {
            await _cache.SetAsync(_cacheKey, items, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(5) });
        }

        private void SetCacheKey(string startDate, string endDate)
        {
            _cacheKey = !string.IsNullOrEmpty(_cacheKey) ? _cacheKey : $"{KeyPrefix}_{startDate}-{endDate}";
        }

        private void SetCacheKey(string companyName, string startDate, string endDate)
        {
            _cacheKey = !string.IsNullOrEmpty(_cacheKey) ? _cacheKey : $"{KeyPrefix}_{companyName}_{startDate}-{endDate}";
        }
    }
}
