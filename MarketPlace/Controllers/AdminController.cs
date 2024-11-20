using MarketPlace.Models;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MarketPlace.Data;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;

        public AdminController(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var viewModel = new AdminDashboardViewModel
            {
                TotalUsers = await _userManager.Users.CountAsync(),
                TotalProducts = await _context.Products.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(),
                // PendingApprovals = await _context.Products.CountAsync(p => !p.IsApproved),
                // TotalMessages = await _context.Messages.CountAsync(),
                // RecentProducts = await _context.Products.OrderByDescending(p => p.CreatedAt).Take(5).ToListAsync(),
                // RecentUsers = await _userManager.Users.OrderByDescending(u => u.RegisteredAt).Take(5).ToListAsync()
            };
            return View(viewModel);
        }

        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> ManageCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
                TempData["Message"] = "User deleted successfully";
            }
            return RedirectToAction(nameof(GetAllUsers));
        }

        // [HttpGet]
        // public async Task<IActionResult> PendingApprovals()
        // {
        //     var pendingProducts = await _context.Products
        //         .Where(p => !p.IsApproved)
        //         .Include(p => p.User)
        //         .ToListAsync();
        //     return View(pendingProducts);
        // }

        // [HttpPost]
        // public async Task<IActionResult> ApproveProduct(int id)
        // {
        //     var product = await _context.Products.FindAsync(id);
        //     if (product != null)
        //     {
        //         product.IsApproved = true;
        //         await _context.SaveChangesAsync();
        //     }
        //     return RedirectToAction(nameof(PendingApprovals));
        // }

        // [HttpGet]
        // public async Task<IActionResult> SystemMessages()
        // {
        //     var messages = await _context.Messages
        //         .Include(m => m.Sender)
        //         .Include(m => m.Receiver)
        //         .OrderByDescending(m => m.SentAt)
        //         .ToListAsync();
        //     return View(messages);
        // }
    }
}