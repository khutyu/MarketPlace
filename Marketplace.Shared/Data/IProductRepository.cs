using MarketPlace.Shared.Models;

namespace MarketPlace.Shared
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        IEnumerable<Product> GetProductstWithCategoryDetails();

    }
}
