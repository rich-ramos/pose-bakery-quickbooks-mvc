using Intuit.Ipp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Services.QBO.Interfaces
{
    public interface IInvoiceServices
    {
        Task<Invoice> GetInvoiceAsync(string id);
        Task<IEnumerable<Invoice>> GetInvoicesByCustomerRefAndDateRangeAsync(string customerRef, string startDate, string endDate);
        Task<IEnumerable<Invoice>> GetInvoicesByDateRangeAsync(string startDate, string endDate);
    }
}
