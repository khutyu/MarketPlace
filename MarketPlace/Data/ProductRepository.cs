using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
namespace MarketPlace.Data
{
    public  class ProductRepository: RepositoryBase<Product>,IProductRepository
    {

        public ProductRepository(AppDbContext context):base(context) 
        {
        }

        public IEnumerable<Product> GetProducttWithCategoryDetails(int id)
        {
            return _context.Products.Include(p => p.Category).Where(p => p.ProductId == id);
        }
    }
}
