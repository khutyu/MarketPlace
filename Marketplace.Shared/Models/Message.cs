using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Shared.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime Timestamp { get; set; }

        public bool IsRead { get; set; }
    }
}