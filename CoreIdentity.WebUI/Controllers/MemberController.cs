using Microsoft.AspNetCore.Mvc;

namespace CoreIdentity.WebUI.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
