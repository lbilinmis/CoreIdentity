using CoreIdentity.WebUI.Areas.Admin.Models;
using CoreIdentity.WebUI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreIdentity.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserList()
        {
            var userList = await _userManager.Users.ToListAsync();
            var userViewModelList = userList.Select(x => new UserViewModel()
            {
                Id = x.Id,
                Email = x.Email,
                UserName = x.UserName,
            }).ToList();

            return View(userViewModelList);
        }
    }
}
