using MarketPlace.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositoryWrapper _Repository;

        public HomeController(IRepositoryWrapper Repository)
        {
            _Repository = Repository;
        }
        public IActionResult Index()
        {
            return View(_Repository._Products.FindAll());

        }
    }
}
