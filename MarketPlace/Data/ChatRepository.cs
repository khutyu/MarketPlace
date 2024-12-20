using MarketPlace.Models;

namespace MarketPlace.Data
{
    public class ChatRepository : RepositoryBase<Chat>, IChatRepository
    {
        public ChatRepository(AppDbContext context) : base(context)
        {
        }


    }
}
