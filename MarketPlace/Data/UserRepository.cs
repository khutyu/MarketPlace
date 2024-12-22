using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Data
{
    public class UserRepository:RepositoryBase<User>,IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }
        public User GetWithAddress(string id)
        {
            return _appDbContext.Users
                .Where(u => u.Id == id)
                .Include(u => u.Address)
                .FirstOrDefault();
        }


        public User GetWithAddressAndReviews(string id)
        {
            return _appDbContext.Users
                .Where(u => u.Id == id)
                .Include(u => u.Address)
                .Include(u => u.Reviews)
                .FirstOrDefault();
        }
        public User GetByNameWithAddressReview(string userName)
        {
            return _appDbContext.Users
                .Where(u => u.UserName == userName)
                .Include(u => u.Address)
                .Include(u => u.Reviews)
                .FirstOrDefault();
        }
    }
}
