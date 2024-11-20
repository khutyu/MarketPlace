using MarketPlace.Models;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace MarketPlace.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, loginModel.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(loginModel.Username);
                    if (await _userManager.IsInRoleAsync(user, "Administrator"))
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    return Redirect(loginModel?.ReturnUrl ?? "/");
                }
                ModelState.AddModelError(string.Empty, "Incorrect username or password.");
            }
            return View(loginModel);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = "/")
        {
            return View(new UserRegistrationViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationViewModel model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Create a new User object based on the model
            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = new Address
                {
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    City = model.City,
                    State = model.State,
                    PostalCode = model.PostalCode,
                    Country = model.Country
                },
            };

            // Try to create the user with the specified password
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assign the user to the "Seller" role
                await _userManager.AddToRoleAsync(user, "Seller");

                // Sign the user in after successful registration
                await _signInManager.SignInAsync(user, isPersistent: false);

                // If there is a return URL, redirect the user back to that page
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                // Redirect to a default page (e.g., Home/Index) after successful registration
                return RedirectToAction("Index", "Home");
            }

            // Add errors to the ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetUrl = Url.Action("ResetPassword", "Account", new { token, email = Email }, Request.Scheme);
                    // Send the reset URL to the user
                }
                return View("ForgotPasswordConfirmation");
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
