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
            var cache = HttpContext.RequestServices.GetService<IDistributedCache>();
            string cacheKey = $"invoices_{startDate}-{endDate}";
            var cachedInvoices = await cache.GetAsync(cacheKey);

            InvoicesViewModel invoicesViewModel = null;
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms;

            if (cachedInvoices == null)
            {
                var invoices = await _invoiceServices.GetInvoicesByDateRangeAsync(startDate, endDate);
                invoicesViewModel = new InvoicesViewModel
                {
                    Invoices = invoices,
                    StartDate = startDate,
                    EndDate = endDate,
                    ItemByQuantity = GetItemByQuantity(invoices)
                };

                using (ms = new MemoryStream())
                {
                    formatter.Serialize(ms, invoicesViewModel);
                    cachedInvoices = ms.ToArray();
                }
                await cache.SetAsync(cacheKey, cachedInvoices);
            }

            using (ms = new MemoryStream(cachedInvoices))
            {
                invoicesViewModel = (InvoicesViewModel)formatter.Deserialize(ms);
            }

            return View("Invoices", invoicesViewModel);
        }

        private IDictionary<string, int> GetItemByQuantity(IEnumerable<Invoice> invoices)
        {
            var itemByQuantityMap = new Dictionary<string, int>();

            void GetLinesFromInvoice(IEnumerable<Invoice> invoices)
            {
                if (invoices.Count() == 0)
                {
                    return;
                }

                var lines = invoices.First().Line;

                void CollectLineValues(IEnumerable<Line> lines)
                {
                    if (lines.Count() == 0)
                    {
                        return;
                    }

                    if (lines.First().AnyIntuitObject is SalesItemLineDetail lineItem)
                    {
                        var itemName = lineItem.ItemRef.name;
                        var itemQyt = ((int)lineItem.Qty);

                        if (!itemByQuantityMap.ContainsKey(itemName))
                        {
                            itemByQuantityMap.Add(itemName, itemQyt);
                        }
                        else
                        {
                            itemByQuantityMap[itemName] += itemQyt;
                        }
                    }
                    CollectLineValues(lines.Skip(1));
                }
                CollectLineValues(lines);

                GetLinesFromInvoice(invoices.Skip(1));
            }
            GetLinesFromInvoice(invoices);

            return itemByQuantityMap;
        }
    }
}
