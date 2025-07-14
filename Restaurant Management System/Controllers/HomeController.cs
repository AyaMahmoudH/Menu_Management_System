using Microsoft.AspNetCore.Mvc;

namespace Restaurant_Management_System.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "MenuItems");
            }

            return View();
        }
    }
}
