namespace MarketPlace.Shared.Models
{
    public class Chat
    {
        public int ChatId { get; set; }
        public IEnumerable<Message> Messages { get; set; }
    }
}
