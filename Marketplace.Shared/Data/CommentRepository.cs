using MarketPlace.Shared;
using MarketPlace.Shared.Models;

namespace MarketPlace.Shared
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
