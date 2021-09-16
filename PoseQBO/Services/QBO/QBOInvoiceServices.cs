using Intuit.Ipp.Data;
using Intuit.Ipp.QueryFilter;
using PoseQBO.Services.QBO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Services.QBO
{
    public class QBOInvoiceServices : IInvoiceServices
    {
        private readonly IApiServices _apiServices;
        private IEnumerable<Invoice> _invoices;

        public QBOInvoiceServices(IApiServices services)
        {
            _apiServices = services;
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesAsync()
        {
            await _apiServices.ApiCall(context =>
            {
                var queryService = new QueryService<Invoice>(context);
                var query = "Select * From Invoice";
                _invoices = queryService.ExecuteIdsQuery(query).ToList();
            });

            return _invoices;
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByDateRangeAsync(string startDate, string endDate)
        {
            await _apiServices.ApiCall(context =>
            {
                var queryService = new QueryService<Invoice>(context);
                var query = $"Select * From Invoice Where MetaData.CreateTime > \'{startDate}\' AND MetaData.CreateTime < \'{endDate}\'";
                _invoices = queryService.ExecuteIdsQuery(query).ToList();
            });

            return _invoices;
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesByIdAndDateRangeAsync(string customerId, string startDate, string endDate)
        {
            await _apiServices.ApiCall(context =>
            {
                var queryService = new QueryService<Invoice>(context);
                var query = $"Select * From Invoice Where id = \'{customerId}\' And MetaData.CreateTime > \'{startDate}\' AND MetaData.CreateTime < \'{endDate}\'";
                _invoices = queryService.ExecuteIdsQuery(query).ToList();
            });

            return _invoices;
        }
    }
}
