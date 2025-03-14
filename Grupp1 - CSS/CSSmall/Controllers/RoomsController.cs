using Microsoft.AspNetCore.Mvc;

namespace CSSmall.Controllers
{
    public class RoomsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DetailsStandard()
        {
            return View();
        }

        public IActionResult DetailsDeluxe()
        {
            return View();
        }

        public IActionResult DetailsSvit() 
        {
            return View();
        }
    }
}
