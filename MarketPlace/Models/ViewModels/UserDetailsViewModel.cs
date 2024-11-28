
using System.Collections.Generic;

namespace MarketPlace.Models.ViewModels
{
    public class UserDetailsViewModel
    {
        public User User { get; set; }
        public IList<string> Roles { get; set; }
    }
}