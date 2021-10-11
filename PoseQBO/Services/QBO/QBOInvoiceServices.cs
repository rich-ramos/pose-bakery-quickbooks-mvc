using Intuit.Ipp.Data;
using Intuit.Ipp.QueryFilter;
using PoseQBO.Services.QBO.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Services.QBO
{
    public class QBOInvoiceServices : IInvoiceServices
    {
        private readonly IApiServices _apiServices;
        private IEnumerable<Invoice> _invoices;
        private Invoice _invoice;

        public QBOInvoiceServices(IApiServices services)
        {
            _apiServices = services;
        }

        public async Task<Invoice> GetInvoiceAsync(string id)
        {
            if (_invoice?.Id != id)
            {
                await _apiServices.ApiCall(context =>
                {
                    var queryService = new QueryService<Invoice>(context);
                    var query = $"Select * From Invoice Where id = \'{id}\'";
                    _invoice = queryService.ExecuteIdsQuery(query).FirstOrDefault();
                });
            }

            return _invoice;
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
