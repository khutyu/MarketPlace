namespace letschat.Data
{
    public class RepositoryWrapper:IRepositoryWrapper
    {
        private AppDbContext _context;
        private IUserRepository _user;
        private IMessageRepository _message;

        public RepositoryWrapper(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public IUserRepository User
        {
            get
            {
                if(_user == null)
                {
                    _user = new UserRepository(_context);
                }
                return User;
            }
        }

        public IMessageRepository Message
        {
            get
            {
                if (_message == null)
                {
                    _message = new MessageRepository(_context);
                }
                return _message;
            }
        }

     
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
