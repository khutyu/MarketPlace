using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
