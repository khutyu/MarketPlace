using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System;
using Marketplace.Data.Models;

namespace Marketplace.Data.Models
{
    public enum Gender
    {
        Male,
        Female,
        PreferNotToSay,
        Other
    }

    public class User : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string SecondName { get; set; }
        public required string LastName { get; set; }
        public required Gender Gender { get; set; }
        public string ProfilePictureContentType { get; set; }
        public byte[] ProfilePicture { get; set; }
        public bool IsDeletionRequested { get; set; } = false;
        public DateTime? DeletionRequestedAt { get; set; }
        public int Ratings { get; set; } = 0;
        public Address Address { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<Chat> Chats { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public bool IsSuspended { get; set; } = false;
    }
}
