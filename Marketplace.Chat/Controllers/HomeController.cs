using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Chat.Controllers
{
    [Area("MarketPlace.Chat")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

}