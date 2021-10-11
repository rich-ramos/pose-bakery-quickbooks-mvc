using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Services.Caching.Interfaces
{
    public interface IInvoiceCacheService
    {
        string CacheKey { get; }

        Task<byte[]> GetInvoiceItemsAsync(string cacheKey);
        Task<byte[]> GetInvoiceItemsAsync(string startDate, string endDate);
        Task SetInvoiceItemsAsync(string startDate, string endDate, byte[] items);
    }
}
