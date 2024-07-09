using Microsoft.AspNetCore.Identity;

namespace MarketPlace.Models
{
    public abstract class User: IdentityUser
    {
      
        public int AddressId { get;set; }

        public Address Address { get; set; }
          
        public IEnumerable<Chat> Chats { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }

}
