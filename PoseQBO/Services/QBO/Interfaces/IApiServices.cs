using Intuit.Ipp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Services.QBO.Interfaces
{
    public interface IApiServices
    {
        Task ApiCall(Action<ServiceContext> apiCallFunction);
    }
}
