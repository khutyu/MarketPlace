using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IWebHostEnvironment _environment;

        public AccountController(IWebHostEnvironment hostingEnvironment, IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
            _environment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string username){
            var _user = await _repositoryWrapper._Users.GetByUsernameAsync(username);
            var user = await _repositoryWrapper._Users.GetUserWithAddressAsync(_user.Id);
            var model = new ProfileViewModel{
                user = user
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> PublicProfile(string username){
            var _user = await _repositoryWrapper._Users.GetByUsernameAsync(username);
            var user = await _repositoryWrapper._Users.GetUserWithAddressAsync(_user.Id);
            var model = new ProfileViewModel{
                user = user
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadProfilePicture(ProfileViewModel model){
            var file = model.ProfilePicture;
            if(file == null || file.Length == 0){
                TempData["ErrorMessage"] = "No file was selected, please try again";
                return RedirectToAction("Profile");
            }
            else{
                try{
                    var user = await _repositoryWrapper._Users.GetByUsernameAsync(User.Identity.Name);
                    var result = await _repositoryWrapper._Users.UpdateProfilePictureAsync(user.Id, file );
                    
                    if(result.Success){
                        TempData["SuccessMessage"] = "Profile picture updated successfully.";
                        return RedirectToAction("profile", new {username = User.Identity.Name});
                    }
                    else{
                        TempData["ErrorMessage"] = "An Error Occurred while updating your profile picture";
                        return RedirectToAction("profile",new {username = User.Identity.Name});
                    }
                
                    }
                catch{
                    TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
                    return RedirectToAction("profile", new {username = User.Identity.Name});
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchUsers(string username)
        {
            var users = await _repositoryWrapper._Users.SearchUsersAsync(username);
            var result = users.Select(user => new
            {
                user.UserName,
                user.Email,
                ProfilePictureUrl = user.ProfilePictureUrl ?? "/images/default-avatar.jpg",
                ListingCount = user.Products.Count(),
            });

            return Json(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadProfileBanner(ProfileViewModel model)
        {
            if (model.ProfileBanner == null || model.ProfileBanner.Length == 0)
            {
                TempData["ErrorMessage"] = "No file was selected, please try again.";
                return RedirectToAction("Profile", new { username = User.Identity.Name });
            }

            try
            {
                var user = await _repositoryWrapper._Users.GetByUsernameAsync(User.Identity.Name);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "An error occurred while updating your profile banner.";
                    return RedirectToAction("Profile", new { username = User.Identity.Name });
                }

                var result = await _repositoryWrapper._Users.UploadProfileBanner(user.Id, model.ProfileBanner);

                if (result.Success)
                {
                    TempData["SuccessMessage"] = "Profile banner updated successfully.";
                    return RedirectToAction("Profile", new { username = User.Identity.Name });
                }
                else
                {
                    TempData["ErrorMessage"] = string.Join(", ", result.Errors ?? new[] { "An error occurred while updating your profile banner." });
                    return RedirectToAction("Profile", new { username = User.Identity.Name });
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
                return RedirectToAction("Profile", new { username = User.Identity.Name });
            }
        }
        public  async Task<IActionResult> GetNotifications(){
            var userNotification = await _repositoryWrapper._Users.GetUserwithNotificationsAsync(User.Identity.Name);
            if(userNotification != null){
                TempData["ErrorMessage"] = "An error Occurred while fetching your notifications";
                return View(userNotification);
            }
            else{
                var result = userNotification.Select(notification => new
                {
                    notification.Message,
                    notification.CreatedAt,
                    notification.IsRead
                });
                return Json(result);
            }
        }

        // [HttpGet]
        // [Authorize]
        // // public async Task<IActionResult> TestGetNotifications()
        // {
        //     // Simulate a user
        //     var fakeUserNotifications = new User
        //     {
        //         Notifications = new List<Notification>
        //         {
        //             new Notification { Message = "Test Message 1", CreatedAt = DateTime.UtcNow, IsRead = false },
        //             new Notification { Message = "Test Message 2", CreatedAt = DateTime.UtcNow, IsRead = true }
        //         }
        //     };

        //     // Mock behavior directly for quick testing
        //     var result = fakeUserNotifications.Notifications.Select(notification => new
        //     {
        //         notification.Message,
        //         notification.CreatedAt,
        //         notification.IsRead
        //     });

        //     return Json(new { success = true, notifications = result });
        // }

}