using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Data
{
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext context) : base(context)
        {

        }
        public IEnumerable<Review> GetUserReviews(string userId)
        {
            if (userId == null)
            {
                return null;
            }
            return _appDbContext.Reviews.Where(r => r.UserId == userId);
        }
    }
}
