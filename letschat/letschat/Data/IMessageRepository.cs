using letschat.Models;

namespace letschat.Data
{
    public interface IMessageRepository:IRepositoryBase<Message>
    {
        IEnumerable<Message> GetConversation(string userId1, string userId2);
    }
}
