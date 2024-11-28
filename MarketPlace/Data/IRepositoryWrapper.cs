﻿namespace MarketPlace.Data
{
     public interface IRepositoryWrapper
    {
        IProductRepository _Products { get; }
        IChatRepository _Chats { get; }
        ICommentRepository _Comments { get; }
        IUserRepository _Users { get; }
        void Save();
    }
}
