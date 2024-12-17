namespace MarketPlace.Models.ViewModels{
    public class AccountSettingsViewModel{
        public User user{get;set;}
        public string CurrentPassword{get; set;}
        public string NewPassword{get;set;}
        public string ConfirmPassword{get;set;}
    }
}