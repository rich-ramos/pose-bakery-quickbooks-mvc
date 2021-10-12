using Intuit.Ipp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Models.ViewModels
{
    [Serializable]
    public class InvoicesViewModel
    {
        public IEnumerable<Invoice> Invoices { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public IDictionary<string, int> ItemByQuantity
        {
            get
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
                GetLinesFromInvoice(Invoices);

                return itemByQuantityMap;
            }
        }
    }
}
