using Azure.Identity;
using MarketPlace.Data;
using MarketPlace.Models;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Web;

namespace MarketPlace.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(IRepositoryWrapper repositoryWrapper, UserManager<User> userManager,
        SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repo = repositoryWrapper;
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
                var result = await _signInManager.PasswordSignInAsync(loginModel.Username,
                                                            loginModel.Password,
                                                            loginModel.RememberMe,
                                                            false);
                if (result.Succeeded)
                {
                    return Redirect(loginModel?.ReturnUrl ?? "/Home/Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
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
                IsAgreedToTerms = model.IsAgreedToTerms,
                Gender = model.gender,
                DateOfBirth = model.DateOfBirth,
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
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            // Attempt to sign the user in after successful registration
            var signInResult = await _repo._UserServices.SignInAsync(model.Username, model.Password);
            if (!signInResult.Success)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                TempData["ErrorMessage"] = "An Error Occured while signing you in";
                return View("LogIn");
            }
            else
            {
                return Redirect(model.ReturnUrl ?? "/Home/Index");
            }
        }
        [HttpGet]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            if (email.IsNullOrEmpty())
            {
                return Json("Email is required");
            }
            var result = await _userManager.FindByEmailAsync(email);
            if (result != null)
            {
                return Json(true);
            }
            return Json(false);
        }
        [HttpGet]
        public async Task<IActionResult> CheckUserNameExists(string username)
        {
            if (username.IsNullOrEmpty())
            {
                return Json("username is required");
            }
            var result = await _userManager.FindByNameAsync(username);
            if (result != null)
            {
                return Json(true);
            }
            return Json(false);
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
            if (ModelState.IsValid && !string.IsNullOrEmpty(Email))
            {
                var result = await _repo._UserServices.SendResetTokenAsync(Email);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "An email has been sent to your address with instructions to reset your password.";
                    return RedirectToAction("LogIn");
                }
                else
                {
                    TempData["ErrorMessage"] = "There was a problem sending the email. Please try again.";
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Settings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _repo._Users.GetWithAddress(userId);

            var model = new AccountSettingsViewModel
            {
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                address = user.Address
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePersonalInfo(AccountSettingsViewModel model)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


                if (model.FirstName.IsNullOrEmpty() || model.LastName.IsNullOrEmpty())
                {
                    return View("Settings", model);
                }

                var user = _repo._Users.GetById(userId);

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.SecondName = model.SecondName;
                    user.LastName = model.LastName;
                }
                else
                {
                    return RedirectToAction("Settings");
                }

                _repo._Users.Update(user);
                _repo.Save();

                TempData["SuccessMessage"] = "Update successful";
                return RedirectToAction("Settings");

            }
            catch
            {
                TempData["ErrroMessage"] = "An Error Occured while updating you details";
                return RedirectToAction("Settings");
            }

        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateAddress(AccountSettingsViewModel model, string username)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (model.address.AddressLine1.IsNullOrEmpty() || model.address.City.IsNullOrEmpty() ||
            model.address.Country.IsNullOrEmpty() || model.address.PostalCode.IsNullOrEmpty())
            {
                TempData["ErrorMessage"] = "Please ensure all required fields are filled out correctly.";
                return View("Settings", model);
            }
            var address = _repo._Addresses.FindByCondition(a => a.UserId == userId).FirstOrDefault();

            if (address != null)
            {
                address.AddressLine1 = model.address.AddressLine1;
                address.City = model.address.City;
                address.Country = model.address.Country;
                address.PostalCode = model.address.PostalCode;
                address.State = model.address.State;
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while updating your address. Please try again.";
                return RedirectToAction("Settings");
            }

            _repo._Addresses.Update(address);
            _repo.Save();
            TempData["SuccessMessage"] = "Address updated successfully.";
            return RedirectToAction("Settings");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateContact(AccountSettingsViewModel model, string username)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (model.Email.IsNullOrEmpty() || model.PhoneNumber.IsNullOrEmpty())
            {
                TempData["ErrorMessage"] = "Please ensure all required fields are filled out correctly.";
                return View("Settings", model);
            }

            var user = _repo._Users.GetById(userId);

            if (user != null)
            {
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while updating your contact details. Please try again.";
                return RedirectToAction("Settings");
            }

            _repo._Users.Update(user);
            _repo.Save();
            TempData["SuccessMessage"] = "Contact details updated successfully.";
            return RedirectToAction("Settings");

        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(AccountSettingsViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (model.CurrentPassword.IsNullOrEmpty() || model.NewPassword.IsNullOrEmpty() || model.ConfirmPassword.IsNullOrEmpty())
            {
                TempData["ErrorMessage"] = "Please ensure all required fields are filled out correctly.";
                return View("Settings", model);
            }
            var user = _repo._Users.GetById(userId);
            var result = _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.IsCompletedSuccessfully)
            {
                TempData["SuccessMessage"] = "Password changed successfully.";
                return RedirectToAction("Settings");
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while updating your password. Please try again.";
                return RedirectToAction("Settings");
            }

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
             await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
