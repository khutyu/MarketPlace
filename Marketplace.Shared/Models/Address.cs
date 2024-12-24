using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Shared.Models
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [Required(ErrorMessage = "Address Line 1 is required.")]
        [Display(Name = "Address Line 1")]
        public string? AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string? AddressLine2 { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string? City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [Display(Name = "State/Province")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Postal code is required.")]
        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        public string? Country { get; set; }

        // Foreign key to User
        [Required]
        [ForeignKey("UserId")]
        public string? UserId { get; set; }

    }
}
