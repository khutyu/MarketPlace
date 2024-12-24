using MarketPlace.Shared.Models;

namespace Marketplace.Chat.Model.ViewModels
{
    public class ChatViewModel
    {
        public User CurrentUser { get; set; }
        public User SelectedContact { get; set; }
        public List<User> Contacts { get; set; }
        public List<Message> Messages { get; set; }
    }
}
