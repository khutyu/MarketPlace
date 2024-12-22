using MarketPlace.Data.Services;

namespace MarketPlace.Data
{
    public interface IRepositoryWrapper
    {
        IProductRepository _Products { get; }
        IChatRepository _Chats { get; }
        ICommentRepository _Comments { get; }
        IUserServices _UserServices { get; }
        ICategoryRepository _Categories { get; }
        IReviewRepository _Reviews { get; }
        IUserRepository _Users { get; }
        IAddressRepository _Addresses { get; }
        void Save();
    }
}
