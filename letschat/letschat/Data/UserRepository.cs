using letschat.Models;
namespace letschat.Data
{
    public class UserRepository: RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext appDbContext)
           : base(appDbContext)
        {
        }

        public User GetByUsername(string username)
        {
           return _appDbContext.Users.FirstOrDefault(x => x.Username == username);
        }
    }
}
