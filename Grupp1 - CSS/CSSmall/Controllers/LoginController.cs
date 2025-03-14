using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "test" && password == "1234")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Fel användarnamn eller lösenord.";
            return RedirectToAction("Index", "Login");
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
