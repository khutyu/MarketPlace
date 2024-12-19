using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketPlace.Data;

namespace MarketPlace.Controllers
{
    [Authorize(Roles = "Administrator")] // Only admins can access this controller
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        protected readonly IRepositoryWrapper _repositoryWrapper;

        public AdminController(UserManager<User> userManager, SignInManager<User> signInManager, IRepositoryWrapper repositoryWrapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repositoryWrapper = repositoryWrapper;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View(new AdminDashboardViewModel
            {
                //totalUsers = _userManager.Users.Count(),
            });
        }
        
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            return View(user);
        }

        // POST: Admin/DeleteUserConfirmed/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return RedirectToAction("Dashboard");

            ModelState.AddModelError("", "Error deleting user.");
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            user.IsSuspended = false;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return RedirectToAction("Dashboard");

            ModelState.AddModelError("", "Error unsuspending user.");
            return RedirectToAction("Dashboard");
        }

        //Get user Details for the _UserDetailsPartial.cshtml
        [HttpGet]
        public async Task<IActionResult> GetUserDetailsPartial(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var viewModel = new UserDetailsViewModel
            {
                User = user,
                Roles = roles
            };

            return PartialView("_UserDetailsPartial", viewModel);
        }
    }
    
}