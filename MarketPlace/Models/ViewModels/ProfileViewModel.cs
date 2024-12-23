namespace MarketPlace.Models
{
    public class ProfileViewModel{
        public User user {get;set;}
        public int AverageRating {get;set;}
        public IFormFile ProfilePicture{get;set;}
        public IFormFile ProfileBanner{get;set;}
        public string ReviewComment {get;set;}
        public int ReviewRating {get;set;}

    }
}