using Microsoft.EntityFrameworkCore;
using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;

namespace MarketPlace.Data
{
    public class UserRepository : RepositoryBase<User>
    {
        
        private readonly UserManager<User> _userManager;

        public UserRepository(AppDbContext appIdentityDbContext,UserManager<User> userManager):base(appIdentityDbContext)
        {
          
            _userManager = userManager;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await FindByCondition(u => u.UserName == username)
                .Include(u => u.Address)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await FindByCondition(u => u.Email == email)
                .Include(u => u.Address)
                .FirstOrDefaultAsync();
        }

        public async Task<(bool Success, IEnumerable<string> Errors)> UpdateProfilePictureAsync(string userId, IFormFile profilePicture)
        {
            var errors = new List<string>();
            var user = await _appIdentityDbContext.Users.FindAsync(userId);
        
            if (user == null)
            {
                errors.Add("User not found");
                return (false, errors);
            }
            // Validate and process the profile picture
            if(profilePicture != null && profilePicture.Length > 0)
            {
                var allowedExtensions = new[] { "jpg", "jpeg", "png" };
                var extension = Path.GetExtension(profilePicture.FileName).Substring(1);

                if (!allowedExtensions.Contains(extension))
                {
                    errors.Add("Invalid file type. Only jpg, jpeg and png files are allowed");
                    return (false, errors);
                }
                else if (profilePicture.Length > 2 * 1024 * 1024)
                {
                    errors.Add("File size exceeds 2MB");
                    return (false, errors);
                }
                var userToUpdate = await _appDbContext.Users.FindAsync(user);
                if(userToUpdate == null)
                {
                    errors.Add("User not found");
                    return (false, errors);
                }
                using (var memoryStream = new MemoryStream())
                {
                    await profilePicture.CopyToAsync(memoryStream);
                    userToUpdate.ProfilePicture = memoryStream.ToArray();
                    userToUpdate.ProfilePictureContentType = profilePicture.ContentType;
                }
                await _appDbContext.SaveChangesAsync();
            }
            return (true, errors);
        }
        public async Task<bool> UpdateUserDetailsAsync(User user)
        {
            User existingUser = FindByCondition(u => u.Id == user.Id)
                .Include(u => u.Address)
                .FirstOrDefault();
            if (existingUser == null) return false;

            _appDbContext.Entry(existingUser).CurrentValues.SetValues(user);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RequestAccountDeletionAsync(string userId)
        {
            var user = FindByCondition(u => u.Id == userId).FirstOrDefault();
            if(user == null)
                return false;

            user.DeletionRequestedAt = DateTime.Now;
            user.IsDeletionRequested = true;
            _appDbContext.Update(user);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAddressAsync(string userId, Address address)
        {
            if(userId == null || address == null)
                return false;

            var user = FindByCondition(u => u.Id == userId)
                .Include(u => u.Address)
                .FirstOrDefault();

            if (user == null){
                return false;
            }

            user.Address = address;
            _appDbContext.Update(user);
            await _appDbContext.SaveChangesAsync();
            return true;

        }

        public async Task<User> GetUserWithAddressAsync(string userId)
        {
            return await FindByCondition(u => u.Id == userId)
                .Include(u => u.Address)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ChangeAccountStatus(string userId)
        {
            var user = await FindByCondition(u => u.Id == userId).FirstOrDefaultAsync();
            if (user == null)
                throw new Exception("User not found");

            user.IsSuspended = !user.IsSuspended; // Toggle status

            _appDbContext.Update(user);
            await _appDbContext.SaveChangesAsync();

            return user.IsSuspended;
        }


        public async Task<(bool Success, IEnumerable<string> Errors)> CreateUserAsync(User user, string password, IFormFile profilePicture = null)
        {
            var errors = new List<string>();

            // Validate user and password input
            if (user == null || string.IsNullOrEmpty(password))
            {
                errors.Add("User or password cannot be null");
                return (false, errors);
            }

            // Create the user
            var result = await _userManager.CreateAsync(user, password);





            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors.Select(e => e.Description)); // Collect identity errors
                return (false, errors);
            }

            // Assign the user to the "Seller" role
            var roleResult = await _userManager.AddToRoleAsync(user, "Seller");
            if (!roleResult.Succeeded)
            {
                errors.AddRange(roleResult.Errors.Select(e => e.Description));
                return (false, errors);
            }

            // Handle profile picture upload if provided
            if (profilePicture != null)
            {
                var (Success, profilePictureErrors) = await UpdateProfilePictureAsync(user.Id, profilePicture);
                if (!Success)
                {
                    errors.AddRange(profilePictureErrors);
                    return (false, errors);
                }
            }

            return (true, errors); // Success
        }

    }
}