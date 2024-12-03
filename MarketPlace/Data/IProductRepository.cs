using MarketPlace.Models;

namespace MarketPlace.Data
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
       IEnumerable<Product> GetProducttWithCategoryDetails(int id);
    }
}
