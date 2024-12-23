namespace letschat.Data
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IMessageRepository Message { get; }
        void Save();
    }
}
