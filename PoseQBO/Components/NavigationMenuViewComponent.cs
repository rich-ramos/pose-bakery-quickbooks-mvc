using Microsoft.AspNetCore.Mvc;
using PoseQBO.Infrastructure;
using PoseQBO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private ILoginResultManager _loginResultManager;

        public NavigationMenuViewComponent(ILoginResultManager loginResultManager)
        {
            _loginResultManager = loginResultManager;
        }

        public IViewComponentResult Invoke()
        {
            return View(_loginResultManager);
        }
    }
}
