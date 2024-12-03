using System.Linq.Expressions;
using MarketPlace.Models;

namespace MarketPlace.Data
{
    public interface IUserRepository: IRepositoryBase<User>
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> GetUserByEmailAsync(string email);
        Task<(bool Success, IEnumerable<string> Errors)> UpdateProfilePictureAsync(string userId, IFormFile profilePicture);
        Task<bool> UpdateUserDetailsAsync(User user);
        Task<bool> RequestAccountDeletionAsync(string userId);
        Task<bool> UpdateAddressAsync(string userId, Address address);
        Task<User> GetUserWithAddressAsync(string userId);
        Task<bool> ChangeAccountStatus(string userId);
        Task<(bool Success, IEnumerable<string> Errors)> CreateUserAsync(User user, string password);
    }
}