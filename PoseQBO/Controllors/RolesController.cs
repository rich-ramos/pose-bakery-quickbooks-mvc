using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PoseQBO.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Controllors
{
    public class RolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult List()
        {
            IEnumerable<IdentityRole> roles = _roleManager.Roles;
            return View(roles);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole { Name = name };
                IdentityResult result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("List");
                foreach (IdentityError err in result.Errors)
                    ModelState.AddModelError("", err.Description);
            }
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            IEnumerable<IdentityUser> members = await _userManager.GetUsersInRoleAsync(role.Name);
            IEnumerable<IdentityUser> nonMembers = _userManager.Users.ToList().Except(members);

            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, string rolename)
        {
            var role = await _roleManager.FindByNameAsync(rolename);
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            IdentityResult result;
            if (await _userManager.IsInRoleAsync(user, rolename))
                result = await _userManager.RemoveFromRoleAsync(user, rolename);
            else
                result = await _userManager.AddToRoleAsync(user, rolename);
            if (result.Succeeded)
                return RedirectToAction();
            else
                foreach (IdentityError err in result.Errors)
                    ModelState.AddModelError("", err.Description);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
            return RedirectToAction("List");
        }
    }
}
