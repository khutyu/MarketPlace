using MarketPlace.Data.Services;

namespace MarketPlace.Data
{
    public interface IRepositoryWrapper
    {
        IProductRepository _Products { get; }
        IChatRepository _Chats { get; }
        ICommentRepository _Comments { get; }
        IUserServices _Users { get; }
        ICategoryRepository _Categories { get; }
        IReviewRepository _Reviews { get; }
        void Save();
    }
}
