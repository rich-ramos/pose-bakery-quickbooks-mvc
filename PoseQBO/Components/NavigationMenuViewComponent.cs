using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {

        public NavigationMenuViewComponent()
        {

        }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
