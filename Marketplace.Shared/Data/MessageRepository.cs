using MarketPlace.Shared.Models;
using MarketPlace.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Shared.Data
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
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
