using Marketplace.Shared.Data;

namespace MarketPlace.Shared
{
    public interface IRepositoryWrapper
    {
        IProductRepository _Products { get; }
        IChatRepository _Chats { get; }
        ICommentRepository _Comments { get; }
        ICategoryRepository _Categories { get; }
        IReviewRepository _Reviews { get; }
        IUserRepository _Users { get; }
        IAddressRepository _Addresses { get; }
        IMessageRepository _Messages { get; }
        void Save();
    }
}
