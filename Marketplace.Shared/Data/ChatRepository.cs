using MarketPlace.Shared;
using MarketPlace.Shared.Models;

namespace MarketPlace.Shared
{
    public class ChatRepository : RepositoryBase<Chat>, IChatRepository
    {
        public ChatRepository(AppDbContext context) : base(context)
        {
        }


    }
}
