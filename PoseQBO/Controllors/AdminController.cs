using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PoseQBO.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Controllors
{
    [Authorize(Roles = "Admins")]
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

        public async Task<IActionResult> Edit(string Id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(Id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string Id,
            string userName,
            string email,
            string password)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByIdAsync(Id);
                user.UserName = userName;
                user.Email = email;
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded && !String.IsNullOrEmpty(password))
                {
                    await _userManager.RemovePasswordAsync(user);
                    result = await _userManager.AddPasswordAsync(user, password);
                }
                if (result.Succeeded)
                    return RedirectToAction("List");
                foreach (IdentityError err in result.Errors)
                    ModelState.AddModelError("", err.Description);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(Id);
            if (user != null)
                await _userManager.DeleteAsync(user);
            return RedirectToAction("List");
        }
    }
}
