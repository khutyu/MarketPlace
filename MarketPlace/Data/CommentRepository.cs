using MarketPlace.Models;

namespace MarketPlace.Data
{
    public class CommentRepository:RepositoryBase<Comment>,ICommentRepository
    {
        public CommentRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
