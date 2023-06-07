using CoreIdentity.WebUI.Areas.Admin.Models;
using CoreIdentity.WebUI.Entities;
using CoreIdentity.WebUI.Extensions;
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

        public IActionResult RoleCreate()
        {
            return View();
        }

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


        public async Task<IActionResult> RoleUpdate(string id)
        {
            var roleUpdate = await _roleManager.FindByIdAsync(id);
            if (roleUpdate == null)
            {
                throw new Exception("Güncellenecek bir role bulunamadı");
            }

            RoleUpdateViewModel model = new RoleUpdateViewModel()
            {
                Id=roleUpdate.Id,
                Name = roleUpdate.Name,
            };
            return View(model);
        }

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

    }
}
