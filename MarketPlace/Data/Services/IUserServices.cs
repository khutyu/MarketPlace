using MarketPlace.Models;

namespace MarketPlace.Data.Services
{
    public interface IUserServices
    {
        Task<(bool Success, IEnumerable<string> Errors)> UpdateProfilePictureAsync(string userId, IFormFile profilePicture);
        Task<(bool Success, IEnumerable<string> Errors)> UploadProfileBanner(string userId, IFormFile profilebanner);
        Task<bool> UpdateUserDetailsAsync(User user);
        Task<(bool Success, List<string> error)> SendResetTokenAsync(string userId);
        Task<bool> ResetPasswordAsync(string email,string token, string newPassword);
        Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<bool> RequestAccountDeletionAsync(string userId);
        Task<User> GetUserWithAddressAsync(string userId);
        Task<bool> ChangeAccountStatus(string userId);
        Task<(bool Success, List<string> message)> CreateUserAsync(User Userdto , string password , IFormFile profilePicture = null);
        Task<(bool Success, List<string> message)> SignInAsync(string email, string password);
        Task<bool> SignOutAsync(string Username);
        Task<IEnumerable<User>> SearchUsersAsync(string username);
        Task<IEnumerable<Notification>> GetUserwithNotificationsAsync(string username);
    }
}
