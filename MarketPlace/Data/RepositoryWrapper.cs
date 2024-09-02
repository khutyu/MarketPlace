using MarketPlace.Data;

namespace ContosoUniversity_ver2.Data
{
    class RepositoryWrapper:IRepositoryWrapper
    {
        private AppDbContext _appDbContext;
      

        public RepositoryWrapper(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public ICommentRepository _Comments
        {
            get
            {
                if (_Comments == null)
                {
                    _Comments = new CommentRepository(_appDbContext);
                }
                return _Comments;
            }
            set { }
            
        }

        public IProductRepository _Products
        {
            get
            {
                if (_Products == null)
                {
                    _Products= new ProductRepository(_appDbContext);
                }
                return _Products;
            }
            set { }
        }

        public IChatRepository _Chats
        {
            get
            {
                if (_Chats == null)
                {
                    _Chats= new ChatRepository(_appDbContext);
                }
                return _Chats;
            }
            set { }
        }


        public void Save()
        {
            _appDbContext.SaveChanges();
        }

    }
}
