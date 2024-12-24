using MarketPlace.Shared.Models;
using Microsoft.EntityFrameworkCore;
namespace MarketPlace.Shared
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {

        public ProductRepository(AppDbContext _context) : base(_context)
        {
        }

        public IEnumerable<Product> GetProductstWithCategoryDetails()
        {
            return _appDbContext.Products.Include(p => p.Category);
        }
    }
}
