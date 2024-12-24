using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Chat.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
