using MarketPlace.Shared.Models;
using Microsoft.EntityFrameworkCore;
namespace MarketPlace.Shared
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {

        public AddressRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
