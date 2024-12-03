using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [UIHint("password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
