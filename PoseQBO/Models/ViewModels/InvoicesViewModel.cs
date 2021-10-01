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
        public IDictionary<string, int> ItemByQuantity { get; set; }
    }
}
