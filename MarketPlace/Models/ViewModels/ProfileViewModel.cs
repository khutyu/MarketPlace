namespace MarketPlace.Models
{
    public class ProfileViewModel{
        public User user {get;set;}
        public IFormFile ProfilePicture{get;set;}
        public IFormFile ProfileBanner{get;set;}
    }
}