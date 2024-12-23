using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using letschat.Models;
using letschat.Models.ViewModels;
using letschat.Data;

namespace letschat.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

}
