using CoreIdentity.WebUI.Entities;
using CoreIdentity.WebUI.Extensions;
using CoreIdentity.WebUI.ViewModels.AppUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreIdentity.WebUI.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            AppUser currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser == null)
            {
                throw new Exception("Kullanıcı bulunamadı");
            }
            LoginUserViewModel user = new LoginUserViewModel
            {
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber,
                UserName = currentUser.UserName,
            };

            return View(user);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task LogOut2()
        {
            await _signInManager.SignOutAsync();
        }


        public IActionResult PasswordChange()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);

            if (currentUser == null)
            {
                throw new Exception("Böyle bir kullanıcı bulunamadı");
            }

            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.OldPassword);

            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski şifreniz yanlış");
            }

            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser, request.OldPassword, request.NewPassword);

            if (!resultChangePassword.Succeeded)
            {

                ModelState.AddModelErrorList(resultChangePassword.Errors.Select(x => x.Description).ToList());
            }

            //security stamp de değiştirilmeli

            await _userManager.UpdateSecurityStampAsync(currentUser);

            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, request.NewPassword, true, false);


            TempData["Success"] = "Şifreniz başarlı şekilde değiştirildi.";

            return View();
        }
    }
}
