using MarketPlace.Models;

namespace MarketPlace.Data
{
     class ProductRepository: RepositoryBase<Product>,IProductRepository
    {

        public ProductRepository(AppDbContext context):base(context) 
        {
        }
    }
}
