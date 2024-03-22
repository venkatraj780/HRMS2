using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
