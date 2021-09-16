using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Services.QBO.Interfaces
{
    public interface ICustomerServices
    {
        Task<string> GetCustomerIdAsync(string customerName);
    }
}
