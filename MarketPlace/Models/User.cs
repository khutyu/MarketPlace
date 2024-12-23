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
        required
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        required
        public string LastName { get; set; }
        required
        public Gender Gender {get;set;}
        required
        public DateTime DateOfBirth {get;set;}
        public string ProfilePictureContentType { get; set; }
        public byte[] ProfilePicture { get; set; }
        public byte[] ProfileBanner { get; set; }
        public  string ProfilePictureUrl { get; set; }
        public string ProfileBannerUrl { get; set; }
        public bool IsDeletionRequested { get; set; } = false;
        public DateTime? DeletionRequestedAt { get; set; }
        public int Rating { get; set; } = 1;
        required
        public Address Address { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Chat> Chats { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public bool IsSuspended { get; set; } = false;
        public IEnumerable<Notification> Notifications { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        required
        public bool IsAgreedToTerms { get; set; }
    }
}
