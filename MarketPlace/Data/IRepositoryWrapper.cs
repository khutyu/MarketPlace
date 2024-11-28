namespace MarketPlace.Data
{
    public interface IRepositoryWrapper
    {
        IProductRepository _Products { get; }
        IChatRepository _Chats { get; }
        ICommentRepository _Comments { get; }
        ICategoryRepository _Categories { get; }
        void Save();
    }
}
