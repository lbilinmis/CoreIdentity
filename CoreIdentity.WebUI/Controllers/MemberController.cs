using CoreIdentity.WebUI.Common;
using CoreIdentity.WebUI.Entities;
using CoreIdentity.WebUI.Extensions;
using CoreIdentity.WebUI.ViewModels.AppUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;

namespace CoreIdentity.WebUI.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFileProvider _fileProvider;
        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IFileProvider fileProvider)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _fileProvider = fileProvider;
        }

        public async Task<IActionResult> Index()
        {
            var userClaims = User.Claims.ToList();
            var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
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
                PictureUrl = currentUser.Picture
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


        public async Task<IActionResult> UserEdit()
        {
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);
            var userEditViewModel = new EditUserViewModel()
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                BirthDate = currentUser.BirthDate,
                City = currentUser.City,
                PhoneNumber = currentUser.PhoneNumber,
                Gender = currentUser.Gender ?? currentUser.Gender
            };
            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(EditUserViewModel request)
        {
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity?.Name);
            currentUser.Email = request.Email;
            currentUser.BirthDate = request.BirthDate;
            currentUser.City = request.City;
            currentUser.UserName = request.UserName;
            currentUser.PhoneNumber = request.PhoneNumber;
            currentUser.Gender = request.Gender;

            if (request.Picture != null && request.Picture.Length > 0)
            {
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");
                var randomFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.Picture.FileName);

                var newPicturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "userpictures").PhysicalPath, randomFileName);

                using (var stream = new FileStream(newPicturePath, FileMode.Create))
                {
                    await request.Picture.CopyToAsync(stream);
                }

                currentUser.Picture = randomFileName;
            }

            var updateResult = await _userManager.UpdateAsync(currentUser);

            if (!updateResult.Succeeded)
            {
                ModelState.AddModelErrorList(updateResult.Errors);
                return View();

            }

            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(currentUser, true);

            TempData["Success"] = "üye bilgileri başarlı şekilde değiştirildi.";
            return View();
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            string message = String.Empty;
            message = "Bu sayfaya erişim yetkiniz yoktur.";
            ViewBag.Message = message;
            return View();
        }

        public IActionResult Claims()
        {
            var userClaimList = User.Claims.Select(x => new ClaimViewModel()
            {

                Provider = x.Issuer,
                Type = x.Type,
                Value = x.Value
            }).ToList();

            return View(userClaimList);
        }


        [Authorize(Policy = "DiyarbakirPolicy")]
        [HttpGet]
        public IActionResult DiyarbakirPolicy()
        {
           
            return View();
        }


        [Authorize(Policy = Constants.PolicyExchange)]
        [HttpGet]
        public IActionResult ExchangePolicyPage()
        {

            return View();
        }
    }
}
