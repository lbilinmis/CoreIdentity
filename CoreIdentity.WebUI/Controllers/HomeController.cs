using CoreIdentity.WebUI.Entities;
using CoreIdentity.WebUI.Extensions;
using CoreIdentity.WebUI.Models;
using CoreIdentity.WebUI.Services.Abstract;
using CoreIdentity.WebUI.ViewModels.AppUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoreIdentity.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = new AppUser()
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                City = "Diyarbakır"
            };
            var identityResult = await _userManager.CreateAsync(user, request.PasswordConfirm);

            if (identityResult.Succeeded)
            {
                TempData["Success"] = "Üye kayıt işleminiz başarılı";

                return RedirectToAction(nameof(HomeController.SignUp));
            }


            ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());

            //foreach (IdentityError item in identityResult.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, item.Description);
            //}

            return View();
        }



        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel request, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Action(nameof(HomeController.Index), returnUrl);

            var IsAvaliableUser = await
                _userManager.FindByEmailAsync(request.Email);

            if (IsAvaliableUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya Parola yanlış");
                return View();
            }

            var _signInResult = await
                //_signInManager.PasswordSignInAsync(IsAvaliableUser, request.Password, request.RememberMe, false);
                _signInManager.PasswordSignInAsync(IsAvaliableUser, request.Password, request.RememberMe, true);
            //kitlenmesi için true yaptık son parametreyi

            if (_signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            if (_signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "3 kez hatalı giriş yapıldı. 3 dk boyunca giriş yapılamaz" });
                return View();
            }
            ModelState.AddModelErrorList(new List<string>() { "Email veya parola yanlış" });

            return View();
        }



        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            //link yolu https://localhost:7132?userId=212121&token=asdfwertzcv

            var hasUser = await _userManager.FindByEmailAsync(request.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu email adresine ait kullanıcı bulunamadı.");
                return View();
            }

            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(hasUser);
            string passwordResetLink =
                Url.Action("ResetPassword", "Home",
                new { userId = hasUser.Id, Token = passwordResetToken }, HttpContext.Request.Scheme);

            ////şağıdaki kod şu an çalışmayacaktır
            //await _emailService.SendResetPasswordEmail(passwordResetLink, request.Email);

            TempData["Success"] = "Şifre resetleme linki e-posta adresinize yönlendirilmiştir.";

            return RedirectToAction(nameof(HomeController.ForgetPassword));
        }



        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId; TempData["token"] = token;


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            var userId = TempData["userId"].ToString(); var token = TempData["token"].ToString();

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                throw new Exception("Bir hata meydana geldi");
            }
            var hasUser = await _userManager.FindByIdAsync(userId);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı");
                return View();
            }

            var result = await _userManager.ResetPasswordAsync(hasUser, token, request.Password);
            if (result.Succeeded)
            {
                TempData["Success"] = "Şifre resetleme işlemi başarılı şekilde yapılmıştır.";
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(hasUser);
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}