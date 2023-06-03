using CoreIdentity.WebUI.Entities;
using CoreIdentity.WebUI.Extensions;
using CoreIdentity.WebUI.Models;
using CoreIdentity.WebUI.ViewModels.AppUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CoreIdentity.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
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
                _signInManager.PasswordSignInAsync(IsAvaliableUser, request.Password, request.RememberMe, false);

            if (_signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            ModelState.AddModelErrorList(new List<string>() { "Email veya Parola yanlış" });

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}