using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;

namespace MarketPlace.Data
{
    public class RepositoryWrapper:IRepositoryWrapper
    {
        private AppDbContext _appDbContext;
        private AppIdentityDbContext _appIdentityDbContext;
        private UserManager<User> _userManager;

        public RepositoryWrapper(AppDbContext appDbContext, AppIdentityDbContext appIdentityDbContext,UserManager<User> userManager)
        {
            _appDbContext = appDbContext;
            _appIdentityDbContext = appIdentityDbContext;
            _userManager = userManager;
        }
        public IUserRepository _Users
        {
            get
            {
                if (_Users == null)
                {
                    _Users = new UserRepository(_appIdentityDbContext,_userManager);
                }
                return _Users;
            }
            set { }
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

        public ICategoryRepository _Category
        {
            get
            {
                if (_Category == null)
                {
                    _Category = new CategoryRepository(_appDbContext);
                }
                return _Category;
            }
            set { }
            
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }

    }
}
