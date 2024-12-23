using letschat.Models;

namespace letschat.Data
{
    public class MessageRepository:RepositoryBase<Message>,IMessageRepository
    {
        public MessageRepository(AppDbContext appDbContext)
          : base(appDbContext)
        {
        }

        public IEnumerable<Message> GetConversation(string userId1, string userId2)
        {
           return _appDbContext.Messages
                   .Where(m =>
                    (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                    (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderBy(m => m.Timestamp)
                .ToList();
        }
    }
}
