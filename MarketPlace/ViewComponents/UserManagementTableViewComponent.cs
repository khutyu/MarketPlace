using MarketPlace.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.ViewComponents
{
    public class UserManagementTableViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public UserManagementTableViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
    }
}