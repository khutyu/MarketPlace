using MarketPlace.Data;
using MarketPlace.Models;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
namespace MarketPlace.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment hostingEnvironment, IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = hostingEnvironment;
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
                    if (user.IsSuspended)
                    {
                        ModelState.AddModelError(string.Empty, "Your account has been suspended. Please contact support.");
                        return View(loginModel);
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Administrator"))
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationViewModel model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                // Provide feedback to the user
                ModelState.AddModelError(string.Empty, "Please ensure all required fields are filled out correctly.");
                return View(model);
            }

            // Create a new User object based on the model
            var user = new User
            {
                UserName = model.Username,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                LastName = model.LastName,
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
            var (success, errors) = await _repositoryWrapper._Users.CreateUserAsync(user, model.Password);

            if (!success)
            {
                // Add errors to the ModelState to display them on the form
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(model);
            }

            // Attempt to sign the user in after successful registration
            try
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            catch (Exception ex)
            {
                // Log the exception (replace with your logging library of choice)
                Console.WriteLine($"Sign-in failed: {ex.Message}");

                // Inform the user without exposing sensitive details
                ModelState.AddModelError(string.Empty, "An error occurred while signing you in. Please try logging in manually.");
                return View(model);
            }

            // Redirect to the specified ReturnUrl or a default page
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            // Redirect to a default page (e.g., Home/Index) after successful registration
            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProfilePicture(string userId){
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && user.ProfilePicture != null){
                return File(user.ProfilePicture, user.ProfilePictureContentType);
            }
            else{
                var defaultImagePath = Path.Combine(_environment.WebRootPath, "images", "default-profile.png");
                var imageData = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                return File(imageData, "image/png");
            }
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string Email)
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

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }      
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string OldPassword, string NewPassword)
        {
            if (ModelState.IsValid)
            {
                // Get the currently logged-in user
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    // Attempt to change the user's password
                    var result = await _userManager.ChangePasswordAsync(user, OldPassword, NewPassword);
                    if (result.Succeeded)
                    {
                        // Redirect to a confirmation view
                        return View("ChangePasswordConfirmation");
                    }

                    // Add errors to the ModelState if the password change failed
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    // User is not logged in, redirect to the login page
                    return RedirectToAction("Login", "Account");
                }
            }

            // If validation fails, reload the ChangePassword view with validation messages
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdatePersonalDetails()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new UpdatePersonalDetailsViewModel
            {
                Email = currentUser.Email,
                PhoneNumber = currentUser.PhoneNumber,
                AddressLine1 = currentUser.Address?.AddressLine1,
                AddressLine2 = currentUser.Address?.AddressLine2,
                City = currentUser.Address?.City,
                State = currentUser.Address?.State,
                PostalCode = currentUser.Address?.PostalCode,
                Country = currentUser.Address?.Country
            };

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> UpdatePersonalDetails(UpdatePersonalDetailsViewModel user)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Update Email
                if (currentUser.Email != user.Email)
                {
                    var emailResult = await _userManager.SetEmailAsync(currentUser, user.Email);
                    if (!emailResult.Succeeded)
                    {
                        foreach (var error in emailResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(user);
                    }
                }

                // Update Phone Number
                if (currentUser.PhoneNumber != user.PhoneNumber)
                {
                    var phoneResult = await _userManager.SetPhoneNumberAsync(currentUser, user.PhoneNumber);
                    if (!phoneResult.Succeeded)
                    {
                        foreach (var error in phoneResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(user);
                    }
                }

                // Update Address
                if (currentUser.Address == null)
                {
                    currentUser.Address = new Address();
                }
                currentUser.Address.AddressLine1 = user.AddressLine1;
                currentUser.Address.AddressLine2 = user.AddressLine2;
                currentUser.Address.City = user.City;
                currentUser.Address.State = user.State;
                currentUser.Address.PostalCode = user.PostalCode;
                currentUser.Address.Country = user.Country;

                // Update user
                var result = await _userManager.UpdateAsync(currentUser);
                if (result.Succeeded)
                {
                    return RedirectToAction("UpdatePersonalDetailsConfirmation");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(user);
        }
        [HttpGet]
        [Authorize]
        public IActionResult DeleteAccount()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccountConfirmed()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // User not found; redirect to login page
                return RedirectToAction("Login", "Account");
            }
            user.IsDeletionRequested = true;
            user.DeletionRequestedAt = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("DeleteAccountConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("DeleteAccount");
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
