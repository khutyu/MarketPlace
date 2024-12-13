namespace MarketPlace.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        public string From {  get; set; }
        public string To { get; set; }
        public bool IsRead { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
