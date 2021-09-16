using Intuit.Ipp.Data;
using Intuit.Ipp.QueryFilter;
using PoseQBO.Services.QBO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Services.QBO
{
    public class QBOCustomerServices : ICustomerServices
    {
        private readonly IApiServices _apiServices;
        private string CustomerId;

        public QBOCustomerServices(IApiServices apiServices)
        {
            _apiServices = apiServices;
        }

        public async Task<string> GetCustomerIdAsync(string customerName)
        {
            await _apiServices.ApiCall(context =>
            {
                var queryService = new QueryService<Customer>(context);
                var query = $"Select Id from Customer Where DisplayName = \'{customerName}\'";
                CustomerId = queryService.ExecuteIdsQuery(query).FirstOrDefault().Id;
            });

            return CustomerId;
        }
    }
}
