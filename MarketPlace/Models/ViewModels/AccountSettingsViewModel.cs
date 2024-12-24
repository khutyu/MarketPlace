using MarketPlace.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models.ViewModels
{
    public class AccountSettingsViewModel{
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Second Name")]
        public string SecondName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public byte[] ProfilePicture { get; set; }

        public string CurrentPassword{get; set;}
        public string NewPassword{get;set;}
        public string ConfirmPassword{get;set;}

        public Address address {get;set;}
    }
}