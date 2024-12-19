namespace MarketPlace.Models
{
    public class Notification
    {
        public string UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }

        public Notification()
        {
            CreatedAt = DateTime.Now;
            IsRead = false;
        }
    }
}