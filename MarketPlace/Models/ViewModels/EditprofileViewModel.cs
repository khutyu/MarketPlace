// EditProfileViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models.ViewModels
{
    public class EditProfileViewModel
    {
        public string Id { get; set; }
        
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

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public byte[] ProfilePicture { get; set; }
    }
}