using Microsoft.AspNetCore.Mvc;
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

        public QBOController(IInvoiceServices invoiceServices)
        {
            _invoiceServices = invoiceServices;
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

        public async Task<IActionResult> InvoicesByDateRange(string startDate, string endDate)
        {
            var invoices = await _invoiceServices.GetInvoicesByDateRangeAsync(startDate, endDate);
            return View("Invoices", invoices);
        }
    }
}
