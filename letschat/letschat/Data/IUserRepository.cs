using letschat.Models;
using System.Linq.Expressions;

namespace letschat.Data
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        User GetByUsername(string username);
    }
}
