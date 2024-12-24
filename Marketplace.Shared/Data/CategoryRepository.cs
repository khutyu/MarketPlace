using MarketPlace.Shared.Models;

namespace MarketPlace.Shared
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
