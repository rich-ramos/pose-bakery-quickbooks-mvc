using Intuit.Ipp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Models.ViewModels
{
    public class InvoiceViewModel
    {
        public Invoice Invoice { get; set; }
        public string ReturnUrl { get; set; }
    }
}
