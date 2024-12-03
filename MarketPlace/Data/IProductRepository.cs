using MarketPlace.Models;

namespace MarketPlace.Data
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        IEnumerable<Product> GetWithCategoryDetailsAsync(int id);
    }
}
