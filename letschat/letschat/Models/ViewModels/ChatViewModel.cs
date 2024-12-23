using System.Collections.Generic;

namespace letschat.Models.ViewModels
{
    public class ChatViewModel
    {
        public User CurrentUser { get; set; }
        public User SelectedContact { get; set; }
        public List<User> Contacts { get; set; }
        public List<Message> Messages { get; set; }
    }
}
