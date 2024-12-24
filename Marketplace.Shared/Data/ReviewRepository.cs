namespace MarketPlace.Shared
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
