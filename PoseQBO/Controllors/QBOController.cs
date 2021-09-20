using Microsoft.AspNetCore.Mvc;
using PoseQBO.Models.ViewModels;
using PoseQBO.Services.QBO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Controllors
{
    public class QBOController : Controller
    {
        private readonly IInvoiceServices _invoiceServices;
        private readonly ICustomerServices _customerServices;

        public QBOController(IInvoiceServices invoiceServices, ICustomerServices customerServices)
        {
            _invoiceServices = invoiceServices;
            _customerServices = customerServices;
        }

        public IActionResult Index()
        {
            return View("QBO");
        }

        public async Task<IActionResult> Invoices()
        {
            var invoices = await _invoiceServices.GetInvoicesAsync();
            return View("Invoices", invoices);
        }

        public async Task<IActionResult> NameAndDateRangeInvoices(string companyName, string startDate, string endDate)
        {
            var customerId = await _customerServices.GetCustomerIdAsync(companyName);
            var invoices = await _invoiceServices.GetInvoicesByIdAndDateRangeAsync(customerId, startDate, endDate);
            return View("Invoices", invoices);
        }

        public async Task<IActionResult> DateRangeInvoices(string startDate, string endDate)
        {
            var invoicesViewModel = new InvoicesViewModel
            {
                Invoices = await _invoiceServices.GetInvoicesByDateRangeAsync(startDate, endDate),
                StartDate = startDate,
                EndDate = endDate
            };
            return View("Invoices", invoicesViewModel);
        }
    }
}
