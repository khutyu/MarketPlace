using MarketPlace.Data.Services;

namespace MarketPlace.Data
{
    public interface IRepositoryWrapper
    {
        IProductRepository _Products { get; }
        IChatRepository _Chats { get; }
        ICommentRepository _Comments { get; }
        ICategoryRepository _Categories { get; }
        IUserServices _Users { get; }
        IAdminUserServices _adminServices { get; }
        void Save();
    }
}
