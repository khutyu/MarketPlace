using Microsoft.AspNetCore.Identity;

namespace MarketPlace.Models
{
    public class User : IdentityUser
    {
        public int Ratings { get; set; } = 0;
        public Address Address { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Chat> Chats { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }

}
