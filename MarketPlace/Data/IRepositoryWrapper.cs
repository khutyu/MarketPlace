namespace MarketPlace.Data
{
     interface IRepositoryWrapper
    {
        IProductRepository _Products { get; }
        IChatRepository _Chats { get; }
        ICommentRepository _Comments { get; }

        void Save();
    }
}
