using MarketPlace.Shared.Models;

namespace MarketPlace.Shared
{
    public interface IUserRepository:IRepositoryBase<User>
    {
        User GetWithAddress(string id);
        User GetWithAddressAndReviews(string id);
        User GetByNameWithAddressReview(string userName);
    }
}
