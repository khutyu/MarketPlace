using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Data.Services
{
    
    public class UserServices : IUserServices
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelper _urlHelper;
        protected UserManager<User> _userManager;
        protected SignInManager<User> _signInManager;
        private IEmailService _emailService;
        
        public UserServices(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService,
        IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory)
        {
            _userManager = userManager;

            _signInManager = signInManager;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;

            var actionContext = new ActionContext(
            _httpContextAccessor.HttpContext,
            _httpContextAccessor.HttpContext.GetRouteData(),
        new ActionDescriptor());

    _urlHelper = urlHelperFactory.GetUrlHelper(actionContext);
        }
        public Task<bool> ChangeAccountStatus(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool Success, List<string> message)> CreateUserAsync(User Userdto ,   string password , IFormFile profilePicture = null)
        {
            List<string> message = new List<string>();  
            if(Userdto == null)
            {
                message.Add("User cannot be null");
                return (false, message);
            }
            var result = await _userManager.CreateAsync(Userdto , password);
            if(result.Succeeded)
            {
                await  _userManager.AddToRoleAsync(Userdto, "Buyer");
                message.Add("User created successfully");
                return (true, message);
            }
            else if(profilePicture != null && result.Succeeded)
            {
                await UpdateProfilePictureAsync(Userdto.Id, profilePicture);
                message.Add("User created successfully");
                return (true, message);
            }

            else
            {
                foreach (var error in result.Errors)
                {
                    message.Add(error.Description);
                }
                return (false, message);
            }
        }
        public async Task<(bool Success, List<string> message)> SignInAsync(string Username, string password)
        {
            List<string> message = new List<string>();
            var user = await _userManager.FindByNameAsync(Username);
            if(user == null)
            {
                message.Add("No user found with the provided username");
                return (false, message);
            }
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if(result.Succeeded)
            {
                message.Add("User signed in successfully");
                return (true,message);
            }
            else
            {
                message.Add( "Incorrect username and password combination");
                return (false,message);
            }
        }

        public async Task<User> GetUserWithAddressAsync(string userId)
        {
            var user = await _userManager.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user;
        }

        public Task<bool> RequestAccountDeletionAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool Success, IEnumerable<string> Errors)> UpdateProfilePictureAsync(string userId, IFormFile profilePicture)
        {
            // Validate user existence.
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, new List<string> { "User not found." });
            }

            // Validate file size (5MB limit).
            if (profilePicture.Length > 5 * 1024 * 1024)
            {
                return (false, new List<string> { "File size exceeds 5MB." });
            }

            // Validate file type.
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(profilePicture.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return (false, new List<string> { "Invalid file type." });
            }

            // Ensure upload directory exists.
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/uploads/profilePictures");
            if (!Directory.Exists(uploadsFolder))
            {
                var dir = Directory.CreateDirectory(uploadsFolder);
            }

            // Save the file.
            var filePath = Path.Combine(uploadsFolder, $"{userId}{fileExtension}");
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePicture.CopyToAsync(stream);
            }

            // Update user profile with file URL.
            user.ProfilePictureUrl = $"/images/uploads/profilePictures/{userId}{fileExtension}";
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return (true, Enumerable.Empty<string>());
            }
            else
            {
                return (false, result.Errors.Select(e => e.Description));
            }
        }
        public async Task<(bool Success, IEnumerable<string> Errors)> UploadProfileBanner(string userId, IFormFile profilebanner)
        {
            // Validate user existence.
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, new List<string> { "User not found." });
            }

            // Validate file size (5MB limit).
            if (profilebanner.Length > 5 * 1024 * 1024)
            {
                return (false, new List<string> { "File size exceeds 5MB." });
            }

            // Validate file type.
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(profilebanner.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return (false, new List<string> { "Invalid file type." });
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
                await profilebanner.CopyToAsync(stream);
            }

            // Update user profile with file URL.
            user.ProfileBannerUrl = $"/images/uploads/ProfileBanners/{userId}{fileExtension}";
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return (true, Enumerable.Empty<string>());
            }
            else
            {
                return (false, result.Errors.Select(e => e.Description));
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
        public async Task<User> GetByEmailAsync(string email)
        {
            var result =  await _userManager.FindByEmailAsync(email);

            if(result != null){
                return result;
            }
            else 
                return null;
        }

        public  async Task<bool> UpdateUserDetailsAsync(User user)
        {
            if(user == null)
            {
                return false;
            }
            var result = await _userManager.UpdateAsync(user);

            if(result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SignOutAsync(string Username)
        {
            if(Username == null)
            {
                return  false;
            }
            await _signInManager.SignOutAsync();
            return true;
            
        }

        public async Task<(bool Success, List<string> error)> SendResetTokenAsync(string email)
        {
            List<string> errors = new List<string>();
            var user = await _userManager.FindByEmailAsync(email);

            if(user == null){
                return (false, new List<string> { "User not found" });
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = _urlHelper.Action("ResetPassword","Account", new {token }, protocol: "https");
            var subject = "Password Reset Request";
            var message = $"<p>Click the link below to reset your password:</p> <a href='{resetLink}'>Reset Password</a>";

            var result = await _emailService.SendEmailAsync(user.Email, subject, message);

            if(result){
                return (true,null);
            }
            else
            {
                errors.Add("Failed to send email");
                return (false, errors);
            }

        }
        public async Task<bool> ResetPasswordAsync(string email,string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return false;
            }
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }
        public async Task<bool> ChangePasswordAsync(string username, string oldPassword, string newPassword){
            var user = await _userManager.FindByNameAsync(username);
            if(user == null){
                return false;
            }
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded;
        }
        public async Task<IEnumerable<User>> SearchUsersAsync(string username)
        {
            return await _userManager.Users
                .Include(u => u.Products)
                .Where(u => u.UserName.Contains(username))
                .ToListAsync();
        }
        public async Task<IEnumerable<Notification>> GetUserwithNotificationsAsync(string username)
        {
            var user = await _userManager.Users
                .Include(u => u.Notifications)
                .FirstOrDefaultAsync(u => u.UserName == username);
        
            return user?.Notifications;
        }
    }
}
