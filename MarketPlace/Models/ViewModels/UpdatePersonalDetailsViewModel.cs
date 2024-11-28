using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models.ViewModels
{
    public class UpdatePersonalDetailsViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        
        // Address fields
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }
        
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }
        
        public string City { get; set; }
        
        public string State { get; set; }
        
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        
        public string Country { get; set; }
    }
}