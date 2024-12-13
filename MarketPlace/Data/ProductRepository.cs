using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
namespace MarketPlace.Data
{
    public  class ProductRepository: RepositoryBase<Product>, IProductRepository
    {

        public ProductRepository(AppDbContext _context):base(_context)
        {
        }

        //public async Task<Product> GetWithCategoryDetailsAsync(int id)
        //{
        //    return await _context.
        //        .Include(p => p.Category) 
        //        .FirstOrDefaultAsync(p => p.Id == id);
        //}

        IEnumerable<Product> IProductRepository.GetWithCategoryDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
