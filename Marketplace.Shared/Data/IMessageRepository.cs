using MarketPlace.Shared.Models;
using MarketPlace.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Shared.Data
{
    public interface IMessageRepository : IRepositoryBase<Message>
    {
        IEnumerable<Message> GetConversation(string userId1, string userId2);
    }
}
