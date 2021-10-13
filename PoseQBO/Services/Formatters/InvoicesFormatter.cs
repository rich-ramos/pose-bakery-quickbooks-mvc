using Intuit.Ipp.Data;
using PoseQBO.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace PoseQBO.Services.Formatters
{
    public class InvoicesFormatter
    {
        private MemoryStream _memoryStream;
        private BinaryFormatter _binaryFormatter;

        public InvoicesFormatter()
        {
            _binaryFormatter = new BinaryFormatter();
        }

        public byte[] Serializer(IEnumerable<Invoice> invoiceData)
        {
            byte[] serializedInvoices;
            using (_memoryStream = new MemoryStream())
            {
                _binaryFormatter.Serialize(_memoryStream, invoiceData);
                return serializedInvoices = _memoryStream.ToArray();
            }
        }

        public IEnumerable<Invoice> Deserialize(byte[] invoiceData)
        {
            IEnumerable<Invoice> invoices;
            using (_memoryStream = new MemoryStream(invoiceData))
            {
                return invoices = (IEnumerable<Invoice>)_binaryFormatter.Deserialize(_memoryStream);
            }
        }
    }
}
