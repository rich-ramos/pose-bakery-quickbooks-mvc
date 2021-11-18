using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PoseQBO.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Controllors
{
    public class AdminController : Controller
    {
        private UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult List() => View(_userManager.Users);

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user =
                    new IdentityUser { UserName = model.UserName, Email = model.Email };
                IdentityResult result =
                    await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                    return RedirectToAction("List");
                foreach (IdentityError err in result.Errors)
                    ModelState.AddModelError("", err.Description);
            }
            return View(model);
        }
    }
}
