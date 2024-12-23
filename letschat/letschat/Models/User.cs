using System;
using System.ComponentModel.DataAnnotations;

namespace letschat.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [StringLength(200)]
        public string ProfilePicture { get; set; }

        public bool IsOnline { get; set; }
        public DateTime LastActive { get; set; }
    }
}
