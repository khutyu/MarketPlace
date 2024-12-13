using MarketPlace.Data;
using MarketPlace.Models;
using MarketPlace.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
            var _user = await _repositoryWrapper._Users.GetByUsernameAsync(username);
            var user = await _repositoryWrapper._Users.GetUserWithAddressAsync(_user.Id);
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
        [Authorize]
        public IActionResult DeleteAccount()
        {
            return View();
        }  

        [HttpGet]
        [Authorize]

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ProfileUpdate(string username)
    {
        var user = await _repositoryWrapper._Users.GetByUsernameAsync(username);
        var userWithAddress = await _repositoryWrapper._Users.GetUserWithAddressAsync(user.Id);
        if (userWithAddress == null)
        {
            return NotFound();
        }

        var model = new EditProfileViewModel()
        {
            Id = userWithAddress.Id,
            FirstName = userWithAddress.FirstName,
            SecondName = userWithAddress.SecondName,
            LastName = userWithAddress.LastName,
            Email = userWithAddress.Email,
            PhoneNumber = userWithAddress.PhoneNumber,
            AddressLine1 = userWithAddress.Address?.AddressLine1,
            AddressLine2 = userWithAddress.Address?.AddressLine2,
            City = userWithAddress.Address?.City,
            State = userWithAddress.Address?.State,
            PostalCode = userWithAddress.Address?.PostalCode,
            Country = userWithAddress.Address?.Country,
            ProfilePicture = userWithAddress.ProfilePicture
        };

        return View(model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateProfile(EditProfileViewModel model){
        try{
            var user = await _repositoryWrapper._Users.GetByUsernameAsync(User.Identity.Name);
        var userWithAddress = await _repositoryWrapper._Users.GetUserWithAddressAsync(user.Id);

        if(userWithAddress == null){
            return NotFound();
        }
        else{
            
        // Update user properties
        userWithAddress.FirstName = model.FirstName;
        userWithAddress.SecondName = model.SecondName;
        userWithAddress.LastName = model.LastName;
        userWithAddress.Email = model.Email;
        userWithAddress.PhoneNumber = model.PhoneNumber;

        // Check if Address exists
        if (userWithAddress.Address != null)
        {
            // Update existing address
            userWithAddress.Address.AddressLine1 = model.AddressLine1;
            userWithAddress.Address.AddressLine2 = model.AddressLine2;
            userWithAddress.Address.City = model.City;
            userWithAddress.Address.State = model.State;
            userWithAddress.Address.PostalCode = model.PostalCode;
            userWithAddress.Address.Country = model.Country;
        }
        else
        {
            // Create new address and associate it with the user
            userWithAddress.Address = new Address
            {
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                City = model.City,
                State = model.State,
                PostalCode = model.PostalCode,
                Country = model.Country,
                UserId = user.Id
            };
        }
            var result = await _repositoryWrapper._Users.UpdateUserDetailsAsync(user);
            if(result){
                return RedirectToAction("Profile", new { username = user.UserName });
            }
            else{
                TempData["ErrorMessage"] = "An error occurred while updating your profile. Please try again.";
                return View(model);
            }

        }
        }
        catch{
            TempData["ErrorMessage"] = "An error occurred while updating your profile. Please try again.";
            return RedirectToAction("Profile", new { username = User.Identity.Name });
        }
    }

    public async Task<IActionResult> UpdateProfilePicture(IFormFile profilePicture)
    {
        var user = await _repositoryWrapper._Users.GetByUsernameAsync(User.Identity.Name);
        var result = await _repositoryWrapper._Users.UpdateProfilePictureAsync(user.Id, profilePicture);
        if (result.Success)
        {
            TempData["SuccessMessage"] = "Profile picture updated successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "An error occurred while updating your profile picture. Please try again.";
        }
        return RedirectToAction("Profile", new { username = user.UserName });

    }

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


        public IActionResult AccessDenied()
        {
            return View();
        }


        
    }
}
