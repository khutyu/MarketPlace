using MarketPlace.Models.ViewModels;
using MarketPlace.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class UserController : Controller
{
    private readonly IRepositoryWrapper _repo;

    public UserController(IRepositoryWrapper repositoryWrapper)
    {
        _repo = repositoryWrapper;
    }

    [HttpGet]
    [Authorize]
    public IActionResult Profile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user =  _repo._Users.GetWithAddressAndReviews(userId);

        var model = new ProfileViewModel
        {
            user = user
        };
        return View("/Views/Account/Profile.cshtml",model);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> PublicProfile(string username)
    {
        var user = _repo._Users.GetByNameWithAddressReview(username);
        var model = new ProfileViewModel
        {
            user = user
        };
        return View("/Views/Account/PublicProfile.cshtml",model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadProfilePicture(ProfileViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var file = model.ProfilePicture;
        if (file == null || file.Length == 0)
        {
            TempData["ErrorMessage"] = "No file was selected, please try again";
            return RedirectToAction("Profile");
        }

        var user = _repo._Users.GetById(userId);
        if (user == null)
        {
            TempData["ErrorMessage"] = "An error occurred while updating your profile picture.";
            return RedirectToAction("Profile");
        }

        // Validate file type.
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(fileExtension))
        {
            return View();
        }

        // Ensure upload directory exists.
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/uploads/ProfilePictures");
        if (!Directory.Exists(uploadsFolder))
        {
            var dir = Directory.CreateDirectory(uploadsFolder);
        }

        // Save the file.
        var filePath = Path.Combine(uploadsFolder, $"{userId}{fileExtension}");
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Update user profile with file URL.
        user.ProfilePictureUrl = $"/images/uploads/ProfilePictures/{userId}{fileExtension}";
        _repo._Users.Update(user);
        _repo.Save();
        TempData["SuccessMessage"] = "Profile picture updated successfully.";
        return RedirectToAction("profile", new { username = User.Identity.Name });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> SearchUsers(string username)
    {
        var users = _repo._Users.FindByCondition(u => u.UserName == username);
        
        var result = users.Select(user => new
        {
            user.FirstName,
            user.LastName,
            user.UserName,
            user.Email,
            ProfilePictureUrl = user.ProfilePictureUrl ?? "~/images/profile-default.webp",
            ListingCount = user.Products?.Count() ?? 0,
        });

        return Json(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadProfileBanner(ProfileViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var file = model.ProfileBanner;

        if (file == null || file.Length == 0)
        {
            TempData["ErrorMessage"] = "No file was selected, please try again.";
            return RedirectToAction("Profile", new { username = User.Identity.Name });
        }

        try
        {
            var user =  _repo._Users.GetById(userId);

            // Validate file type.
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                TempData["ErrorMessage"] = "Invalid filetype";
            }

            // Ensure upload directory exists.
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/uploads/ProfileBanners");
            if (!Directory.Exists(uploadsFolder))
            {
                var dir = Directory.CreateDirectory(uploadsFolder);
            }

            // Save the file.
            var filePath = Path.Combine(uploadsFolder, $"{userId}{fileExtension}");
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Update user profile with file URL.
            user.ProfileBannerUrl = $"/images/uploads/ProfileBanners/{userId}{fileExtension}";
            _repo._Users.Update(user);
            _repo.Save();

            TempData["SuccessMessage"] = "Profile banner updated successfully.";
            return RedirectToAction("Profile", new { username = User.Identity.Name });

        }
        catch
        {
            TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
            return RedirectToAction("Profile", new { username = User.Identity.Name });
        }
    }
    //public async Task<IActionResult> GetNotifications()
    //{
    //    var userNotification = await _repo._UserServices.GetUserwithNotificationsAsync(User.Identity.Name);
    //    if (userNotification != null)
    //    {
    //        TempData["ErrorMessage"] = "An error Occurred while fetching your notifications";
    //        return View(userNotification);
    //    }
    //    else
    //    {
    //        var result = userNotification.Select(notification => new
    //        {
    //            notification.Message,
    //            notification.CreatedAt,
    //            notification.IsRead
    //        });
    //        return Json(result);
    //    }
    //}

    [Authorize]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> PostReview(string id, ProfileViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = _repo._Users.GetWithAddressAndReviews(userId);

        int averageRating = 0;
        int count = user.Reviews.Count();

        if(count > 0)
        {
            user.Rating = user.Reviews.Sum(r => r.Rating) / count;
        }

        // Validate user and input data
        if (id == null || string.IsNullOrEmpty(id))
        {
            TempData["ErrorMessage"] = "An error occurred while posting your review.";
            return RedirectToAction("PublicProfile", new { username = User.Identity.Name } ); 
        }

        try{
             // Create a new review
            var newReview = new Review
            {
                UserId = id,
                PosterId = userId,
                Rating = model.ReviewRating,
                Comment = model.ReviewComment,
                CreatedAt = DateTime.UtcNow
            };

            // Add the review to the database
            _repo._Reviews.Create(newReview);
            _repo.Save();

            // Success message
            TempData["SuccessMessage"] = "Review posted successfully!";
            return RedirectToAction("PublicProfile",new { username = User.Identity.Name });
        }
        catch{
            TempData["ErrorMessage"] = "An error occurred while posting your review.";
            return RedirectToAction("PublicProfile", new { username = User.Identity.Name } ); 
        }
    }
}