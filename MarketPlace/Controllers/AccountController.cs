using Azure.Identity;
using MarketPlace.Data;
using MarketPlace.Models;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Web;

namespace MarketPlace.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;

        public AccountController(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
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
                    return RedirectToAction("Index", "Home");
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
            var result  = await _repositoryWrapper._Users.CreateUserAsync(user, model.Password);

            if(!result.Success)
            if(!result.Success)
            {
                // Provide feedback to the user
                foreach (var error in result.message)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            // Attempt to sign the user in after successful registration
            var signInResult = await _repositoryWrapper._Users.SignInAsync(model.Username, model.Password);
            if (!signInResult.Success)
            {
                // Provide feedback to the user
                foreach (var error in signInResult.message)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
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
        public async Task<IActionResult> CheckEmailExists(string email){
            if(email.IsNullOrEmpty()){
                return Json("Email is required");
            }
            var result =  await _repositoryWrapper._Users.GetByEmailAsync(email);
            if(result != null){
                return Json(true);
            }
            return Json(false);
        }
        [HttpGet]
        public async Task<IActionResult> CheckUserNameExists(string username){
            if(username.IsNullOrEmpty()){
                return Json("username is required");
            }
            var result =  await _repositoryWrapper._Users.GetByUsernameAsync(username);
            if(result != null){
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
        public async Task<IActionResult> ChangePasswordConfirmed(ChangePasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _repositoryWrapper._Users.ChangePasswordAsync(User.Identity.Name, model.OldPassword, model.NewPassword);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "Password changed successfully.";
                        return RedirectToAction("Profile", new { username = User.Identity.Name });
                    }
                    return View(model);
                }
                else
                {
                    return View("ChangePassword");
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "An error occurred while changing your password. Please try again.";
                return View("ChangePassword");
            }
        }
        
        [HttpGet]
        [Route("Account/Settings")]
        public async Task<IActionResult> Settings(string username)
        {
            var User = await _repositoryWrapper._Users.GetByUsernameAsync(username);
            var user = await _repositoryWrapper._Users.GetUserWithAddressAsync(User.Id);

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
        [Route("Account/Settings/PersonalInfo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePersonalInfo(AccountSettingsViewModel model,string username)
        {

            if (model.FirstName.IsNullOrEmpty() || model.LastName.IsNullOrEmpty())
            {
                return View("Settings", model);
            }
            var User = await _repositoryWrapper._Users.GetByUsernameAsync(username);
            var user = await _repositoryWrapper._Users.GetUserWithAddressAsync(User.Id);

            if(user != null){
                user.FirstName = model.FirstName;
                user.SecondName = model.SecondName;
                user.LastName = model.LastName;
            }
            else{
                return RedirectToAction("Settings", new { username = new { username = user.UserName } });
            }

            var result = await _repositoryWrapper._Users.UpdateUserDetailsAsync(user);
            if(result){
                TempData["Result"] = "Update successful";
                return RedirectToAction("Settings", new { username = user.UserName });
            }
            return RedirectToAction("Settings", new { username = user.UserName });

        }
        [Route("Account/Settings/Address")]
        [HttpPost]
        public async Task<IActionResult> UpdateAddress(AccountSettingsViewModel model,string username)
        {

            if (model.address.AddressLine1.IsNullOrEmpty() || model.address.City.IsNullOrEmpty() ||
            model.address.Country.IsNullOrEmpty() || model.address.PostalCode.IsNullOrEmpty())
            {
                return View("Settings", model);
            }
            var User = await _repositoryWrapper._Users.GetByUsernameAsync(username);
            var user = await _repositoryWrapper._Users.GetUserWithAddressAsync(User.Id);

            if(user != null){
                user.Address.AddressLine1 = model.address.AddressLine1;
                user.Address.City = model.address.City;
                user.Address.Country = model.address.Country;
                user.Address.PostalCode = model.address.PostalCode;
                

            }
            else{
                return RedirectToAction("Settings", new { username = user.UserName });
            }

            var result = await _repositoryWrapper._Users.UpdateUserDetailsAsync(user);
            if(result){
                TempData["Result"] = "Update successful";
                return RedirectToAction("Settings", new { username = user.UserName });
            }
            return RedirectToAction("Settings", new { username = user.UserName });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Account/Settings/Contact")]
        public async Task<IActionResult> UpdateContact(AccountSettingsViewModel model,string username)
        {

            if (model.Email.IsNullOrEmpty() || model.PhoneNumber.IsNullOrEmpty())
            {
                return View("Settings", model);
            }
            var User = await _repositoryWrapper._Users.GetByUsernameAsync(username);
            var user = await _repositoryWrapper._Users.GetUserWithAddressAsync(User.Id);

            if(user != null){
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
            }
            else{
                return RedirectToAction("Settings", new { username = user.UserName });
            }

            var result = await _repositoryWrapper._Users.UpdateUserDetailsAsync(user);
            if(result){
                TempData["Result"] = "Update successful";
                return RedirectToAction("Settings", new { username = user.UserName });
            }
            return RedirectToAction("Settings", new { username = user.UserName });

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Account/Settings/Password")]
        public async Task<IActionResult> UpdatePassword(AccountSettingsViewModel model)
            {
                try
                {
                    var result = await _repositoryWrapper._Users.ChangePasswordAsync(User.Identity.Name, model.CurrentPassword, model.NewPassword);
                        if (result)
                        {
                            TempData["SuccessMessage"] = "Password changed successfully.";
                            return RedirectToAction("Settings", new { username = User.Identity.Name });
                        }
                        return View(model);
                }
                catch
                {
                    TempData["ErrorMessage"] = "An error occurred while changing your password. Please try again.";
                    return View("ChangePassword");
                }
            }


        [HttpGet]
        [Authorize]
        public IActionResult DeleteAccount()
        {
            return View();
        }  

        [HttpGet]
        [Authorize]

        //Actions for Account settings
        public IActionResult Settings(){
            return View();
        }
        
        [HttpPost]
        [Authorize]
            public async Task<IActionResult> Logout()
            {
                await _repositoryWrapper._Users.SignOutAsync(User.Identity.Name);
                return RedirectToAction("Index", "Home");
            }
        }
}
