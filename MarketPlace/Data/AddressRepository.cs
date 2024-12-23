using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
namespace MarketPlace.Data
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {

        public AddressRepository(AppDbContext _context) : base(_context)
        {
        }
    }
}
