using Intuit.Ipp.Data;
using Microsoft.AspNetCore.Mvc;
using PoseQBO.Models.ViewModels;
using PoseQBO.Services.QBO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using PoseQBO.Services.Caching.Interfaces;
using PoseQBO.Services.Formatters;

namespace PoseQBO.Controllors
{
    public class QBOController : Controller
    {
        private readonly IInvoiceServices _invoiceServices;
        private readonly ICustomerServices _customerServices;
        private readonly IInvoiceCacheService _invoiceCacheService;
        private readonly InvoicesFormatter _invoicesFormatter;

        public QBOController(IInvoiceServices invoiceServices, ICustomerServices customerServices, IInvoiceCacheService invoiceCacheService, InvoicesFormatter invoicesFormatter)
        {
            _invoiceServices = invoiceServices;
            _customerServices = customerServices;
            _invoiceCacheService = invoiceCacheService;
            _invoicesFormatter = invoicesFormatter;
        }

        public IActionResult Index()
        {
            return View("QBO");
        }

        public async Task<IActionResult> Invoice(string id, string returnUrl, string cacheKey)
        {
            Invoice invoice;
            var cachedInvoices = await _invoiceCacheService.GetInvoiceItemsAsync(cacheKey);
            if (cachedInvoices == null)
            {
                invoice = await _invoiceServices.GetInvoiceAsync(id);
            }
            else
            {
                invoice = _invoicesFormatter.Deserialize(cachedInvoices).FirstOrDefault(invoice => invoice.Id == id);
            }

            InvoiceViewModel invoiceViewModel = new InvoiceViewModel
            {
                Invoice = invoice,
                ReturnUrl = returnUrl
            };

            return View(invoiceViewModel);
        }

        public async Task<IActionResult> InvoicesByNameAndDateRange(string companyName, string startDate, string endDate)
        {
            var customerId = await _customerServices.GetCustomerIdAsync(companyName);
            var invoices = await _invoiceServices.GetInvoicesByIdAndDateRangeAsync(customerId, startDate, endDate);
            return View("Invoices", invoices);
        }

        public async Task<IActionResult> InvoicesByDateRange(string startDate, string endDate)
        {
            IEnumerable<Invoice> invoices;
            var cachedInvoices = await _invoiceCacheService.GetInvoiceItemsAsync(startDate, endDate);
            if (cachedInvoices == null)
            {
                invoices = await _invoiceServices.GetInvoicesByDateRangeAsync(startDate, endDate);
                var serializedInvoices = _invoicesFormatter.Serializer(invoices);
                await _invoiceCacheService.SetInvoiceItemsAsync(startDate, endDate, serializedInvoices);
            }
            else
            {
                invoices = _invoicesFormatter.Deserialize(cachedInvoices);
            }

            InvoicesViewModel invoicesViewModel = new InvoicesViewModel
            {
                Invoices = invoices,
                StartDate = startDate,
                EndDate = endDate
            };

            ViewData["CacheKey"] = _invoiceCacheService.CacheKey;

            return View("Invoices", invoicesViewModel);
        }
    }
}
