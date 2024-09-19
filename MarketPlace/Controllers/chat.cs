using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controllers
{
    public class chat : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
