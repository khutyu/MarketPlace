using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using MarketPlace.Data.Services;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly AppDbContext _appDbContext;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUrlHelperFactory _urlHelperFactory;

    // Backing fields for repositories
    private ICommentRepository _comments;
    private IProductRepository _products;
    private IChatRepository _chats;
    private ICategoryRepository _categories;
    private IAdminUserServices _adminServices;
    private IReviewRepository _reviewRepository;
    private IUserRepository _user;
    private IAddressRepository _address;

    public RepositoryWrapper(
        AppDbContext appDbContext,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor,
        IUrlHelperFactory urlHelperFactory)
    {
        _appDbContext = appDbContext;
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
        _urlHelperFactory = urlHelperFactory;

        _UserServices = new UserServices(_userManager, _signInManager, _emailService, _httpContextAccessor, _urlHelperFactory);
    }

    public IUserServices _UserServices { get; }
    public IUserRepository _Users
    {
        get
        {
            if (_user == null)
            {
                _user = new UserRepository(_appDbContext);
            }
            return _user;
        }
        set { _user = value; }
    }

    public ICommentRepository _Comments
    {
        get
        {
            if (_comments == null)
            {
                _comments = new CommentRepository(_appDbContext);
            }
            return _comments;
        }
        set { _comments = value; }
    }

    public IProductRepository _Products
    {
        get
        {
            if (_products == null)
            {
                _products = new ProductRepository(_appDbContext);
            }
            return _products;
        }
        set { _products = value; }
    }

    public IChatRepository _Chats
    {
        get
        {
            if (_chats == null)
            {
                _chats = new ChatRepository(_appDbContext);
            }
            return _chats;
        }
        set { _chats = value; }
    }

    public ICategoryRepository _Categories 
    {
        get{
            if (_categories == null){
                _categories = new CategoryRepository(_appDbContext);
            }
            return _categories;
        }
        set{_categories = value;}
    }

    public IReviewRepository _Reviews
    {
        get
        {
            if(_reviewRepository == null)
            {
                _reviewRepository = new ReviewRepository(_appDbContext);
            }
            return _reviewRepository;
        }
        set {_reviewRepository = value; }
    }

    public IAddressRepository _Addresses
    {
        get
        {
            if (_address == null)
            {
                _address = new AddressRepository(_appDbContext);
            }
            return _address;
        }
        set { _address = value; }
    }

    public void Save()
    {
        _appDbContext.SaveChanges();
    }
}
