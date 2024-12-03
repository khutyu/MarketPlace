using MarketPlace.Data;
using MarketPlace.Models;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Identity.Client;
namespace MarketPlace.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IWebHostEnvironment _environment;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment hostingEnvironment, IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
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
                var result = await _repositoryWrapper._Users.SignInAsync(loginModel.Username, loginModel.Password);
                if (result.Success)
                {
                    if (!string.IsNullOrEmpty(loginModel.ReturnUrl) && Url.IsLocalUrl(loginModel.ReturnUrl))
                    {
                        return Redirect(loginModel.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else{
                    foreach (var error in result.message)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
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
                var result  = await _repositoryWrapper._Users.CreateUserAsync(user, model.Password);

            if(!result.Success)
            {
                // Provide feedback to the user
                foreach (var error in result.message)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            // Attempt to sign the user in after successful registration
            var signInResult = await _repositoryWrapper._Users.SignInAsync(model.Email, model.Password);
            if (!signInResult.Success)
            {
                // Provide feedback to the user
                foreach (var error in signInResult.message)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(model);
                
            }
            else{
                if(!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl)){
                    return Redirect(model.ReturnUrl);
                }
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string username){
            var user = await _repositoryWrapper._Users.GetByUsernameAsync(username);
            return View(user);
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
                var result =  await _repositoryWrapper._Users.SendResetTokenAsync(Email);

                if(result.Success){
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
        
        public async Task<IActionResult> ResetPasswordComfirmed(ResetPasswordViewModel model)
        {
            if(!ModelState.IsValid){
                return View(model);
            }
            else if (!string.IsNullOrEmpty(model.Token) && !string.IsNullOrEmpty(model.Password)){
                var result = await _repositoryWrapper._Users.ResetPasswordAsync(model.Email,model.Token, model.Password);
                if(result){
                    TempData["SuccessMessage"] = "Your password has been reset.";
                    return RedirectToAction("LogIn");
                }
            }
            return View("LogIn");
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }      
                // [HttpPost]
        // [ValidateAntiForgeryToken]
        // [Authorize]
        // public async Task<IActionResult> UpdatePersonalDetails(UpdatePersonalDetailsViewModel user)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var currentUser = await _userManager.GetUserAsync(User);
        //         if (currentUser == null)
        //         {
        //             return RedirectToAction("Login", "Account");
        //         }

        //         // Update Email
        //         if (currentUser.Email != user.Email)
        //         {
        //             var emailResult = await _userManager.SetEmailAsync(currentUser, user.Email);
        //             if (!emailResult.Succeeded)
        //             {
        //                 foreach (var error in emailResult.Errors)
        //                 {
        //                     ModelState.AddModelError(string.Empty, error.Description);
        //                 }
        //                 return View(user);
        //             }
        //         }

        //         // Update Phone Number
        //         if (currentUser.PhoneNumber != user.PhoneNumber)
        //         {
        //             var phoneResult = await _userManager.SetPhoneNumberAsync(currentUser, user.PhoneNumber);
        //             if (!phoneResult.Succeeded)
        //             {
        //                 foreach (var error in phoneResult.Errors)
        //                 {
        //                     ModelState.AddModelError(string.Empty, error.Description);
        //                 }
        //                 return View(user);
        //             }
        //         }

        //         // Update Address
        //         if (currentUser.Address == null)
        //         {
        //             currentUser.Address = new Address();
        //         }
        //         currentUser.Address.AddressLine1 = user.AddressLine1;
        //         currentUser.Address.AddressLine2 = user.AddressLine2;
        //         currentUser.Address.City = user.City;
        //         currentUser.Address.State = user.State;
        //         currentUser.Address.PostalCode = user.PostalCode;
        //         currentUser.Address.Country = user.Country;

        //         // Update user
        //         var result = await _userManager.UpdateAsync(currentUser);
        //         if (result.Succeeded)
        //         {
        //             return RedirectToAction("UpdatePersonalDetailsConfirmation");
        //         }
        //         foreach (var error in result.Errors)
        //         {
        //             ModelState.AddModelError(string.Empty, error.Description);
        //         }
        //     }
        //     return View(user);
        // }
        [HttpGet]
        [Authorize]
        public IActionResult DeleteAccount()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteAccountConfirmed()
        // {
        //     var user = await _userManager.GetUserAsync(User);

        //     if (user == null)
        //     {
        //         // User not found; redirect to login page
        //         return RedirectToAction("Login", "Account");
        //     }
        //     user.IsDeletionRequested = true;
        //     user.DeletionRequestedAt = DateTime.Now;

        //     var result = await _userManager.UpdateAsync(user);

        //     if (result.Succeeded)
        //     {
        //         await _signInManager.SignOutAsync();
        //         return RedirectToAction("DeleteAccountConfirmation");
        //     }
        //     foreach (var error in result.Errors)
        //     {
        //         ModelState.AddModelError(string.Empty, error.Description);
        //     }

        //     return View("DeleteAccount");
        // }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _repositoryWrapper._Users.SignOutAsync(User.Identity.Name);
            return RedirectToAction("Index", "Home");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
