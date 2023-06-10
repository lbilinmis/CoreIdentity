using CoreIdentity.WebUI.Areas.Admin.Models;
using CoreIdentity.WebUI.Entities;
using CoreIdentity.WebUI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreIdentity.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _roleManager.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return View(result);
        }

        [Authorize(Roles = "Admin,role-action")]
        public IActionResult RoleCreate()
        {
            return View();
        }

        [Authorize(Roles = "Admin,role-action")]
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleCreateViewModel request)
        {
            AppRole role = new AppRole();
            role.Name = request.Name;
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();

            }
            return RedirectToAction(nameof(RolesController.Index));
        }

        [Authorize(Roles = "Admin,role-action")]
        public async Task<IActionResult> RoleUpdate(string id)
        {
            var roleUpdate = await _roleManager.FindByIdAsync(id);
            if (roleUpdate == null)
            {
                throw new Exception("Güncellenecek bir role bulunamadı");
            }

            RoleUpdateViewModel model = new RoleUpdateViewModel()
            {
                Id = roleUpdate.Id,
                Name = roleUpdate.Name,
            };
            return View(model);
        }

        [Authorize(Roles = "Admin,role-action")]
        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel request)
        {
            var roleUpdateModel = await _roleManager.FindByIdAsync(request.Id);

            if (roleUpdateModel == null)
            {
                throw new Exception("Güncellenecek bir role bulunamadı");
            }

            roleUpdateModel.Name = request.Name;

            var result = await _roleManager.UpdateAsync(roleUpdateModel);

            if (!result.Succeeded)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();

            }

            TempData["Success"] = "Role güncelleme işleminiz başarılı";
            return RedirectToAction(nameof(RolesController.Index));
        }

        [Authorize(Roles = "Admin,role-action")]

        public async Task<IActionResult> Delete(string id)
        {
            var deleteRole = await _roleManager.FindByIdAsync(id);
            if (deleteRole == null)
            {
                throw new Exception("Güncellenecek bir role bulunamadı");
            }

            await _roleManager.DeleteAsync(deleteRole);

            TempData["Success"] = "Role silme işleminiz başarılı";
            return RedirectToAction(nameof(RolesController.Index));
        }

        public async Task<IActionResult> AssignRoleToUser(string id)
        {
            var currentUser = await _userManager.FindByIdAsync(id);
            ViewBag.UserId = currentUser.Id;
            var roles = await _roleManager.Roles.ToListAsync();

            var roleViewModelList = new List<AssignRoleToUserViewModel>();

            var userRoles = await _userManager.GetRolesAsync(currentUser);

            foreach (var role in roles)
            {
                var assignRoleToUserViewModel = new AssignRoleToUserViewModel();

                assignRoleToUserViewModel.Id = role.Id;
                assignRoleToUserViewModel.Name = role.Name;

                if (userRoles.Contains(role.Name))
                {
                    assignRoleToUserViewModel.Exist = true;
                }

                roleViewModelList.Add(assignRoleToUserViewModel);
            }

            return View(roleViewModelList);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser(string userId, List<AssignRoleToUserViewModel> requestList)
        {
            var user = await _userManager.FindByIdAsync(userId);

            foreach (var item in requestList)
            {
                if (item.Exist)
                {
                    await _userManager.AddToRoleAsync(user, item.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.Name);
                }
            }
            return RedirectToAction(nameof(HomeController.UserList), "Home");
        }
    }
}
