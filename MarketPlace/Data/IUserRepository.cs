using MarketPlace.Models;

namespace MarketPlace.Data
{
    public interface IUserRepository:IRepositoryBase<User>
    {
        User GetWithAddress(string id);
        User GetWithAddressAndReviews(string id);
        User GetByNameWithAddressReview(string userName);
    }
}
