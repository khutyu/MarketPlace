using Microsoft.AspNetCore.Identity;

namespace MarketPlace.Models
{
    public enum Gender{
        Male,
        Female,
        NonBinary,
        PreferNotToSay,
        Other
    }
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public Gender Gender {get;set;}
        public string ProfilePictureContentType { get; set; }
        public byte[] ProfilePicture { get; set; }
        public bool IsDeletionRequested { get; set; } = false;
        public DateTime? DeletionRequestedAt { get; set; }
        public int Ratings { get; set; } = 0;
        public Address Address { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Chat> Chats { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public bool IsSuspended { get; set; } = false;
    }

}
